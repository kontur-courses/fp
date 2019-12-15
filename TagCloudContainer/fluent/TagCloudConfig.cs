using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using TagCloudContainer.Api;
using TagCloudContainer.Implementations;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.fluent
{
    public class TagCloudConfig
    {
        public Result<Func<string, IWordProvider>> WordProvider;
        public ImageFormat ImageFormat;
        public DrawingOptions Options;
        public Type BrushProvider;
        public Type CloudLayouter;
        public Type PenProvider;
        public Type SizeProvider;
        public Type WordCloudLayouter;
        public Type WordProcessor;
        public Type WordVisualizer;

        private TagCloudConfig(TagCloudConfig parent)
        {
            WordProvider = parent.WordProvider;
            WordProcessor = parent.WordProcessor;
            WordVisualizer = parent.WordVisualizer;
            CloudLayouter = parent.CloudLayouter;
            WordCloudLayouter = parent.WordCloudLayouter;
            SizeProvider = parent.SizeProvider;
            BrushProvider = parent.BrushProvider;
            ImageFormat = parent.ImageFormat;
            Options = parent.Options;
        }

        public TagCloudConfig(Cli cli)
        {
            ImageFormat = ImageFormat.Png;
            Options = new DrawingOptions();
            WordProvider = cli.GetWordProviderByName("txt");
            WordProcessor = typeof(LowercaseWordProcessor);
            CloudLayouter = typeof(CircularCloudLayouter);
            SizeProvider = typeof(StringSizeProvider);
            BrushProvider = typeof(OneColorBrushProvider);
            PenProvider = typeof(OneColorPenProvider);
            WordCloudLayouter = typeof(WordCloudLayouter);
            WordVisualizer = typeof(TagCloudVisualizer);
        }

        public Result<TagCloudConfig> UsingWordProvider(string wordProvider, Cli cli)
        {
            WordProvider = cli.GetWordProviderByName(wordProvider);
            return this;
        }

        public Result<TagCloudConfig> UsingWordProcessor(string wordProcessor, Cli cli)
        {
            var wordProcessorResult = cli.GetTypeByCliElementName<IWordVisualizer>(wordProcessor);

            return wordProcessorResult.Then(r =>
            {
                WordProcessor = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingWordVisualizer(string wordVisualizer, Cli cli)
        {
            var wordVisualizerResult = cli.GetTypeByCliElementName<IWordVisualizer>(wordVisualizer);

            return wordVisualizerResult.Then(r =>
            {
                WordVisualizer = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingCloudLayouter(string cloudLayouter, Cli cli)
        {
            var cloudLayouterResult = cli.GetTypeByCliElementName<ICloudLayouter>(cloudLayouter);

            return cloudLayouterResult.Then(r =>
            {
                CloudLayouter = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingWordCloudLayouter(string wordCloudLayouter, Cli cli)
        {
            var wordCloudLayouterResult = cli.GetTypeByCliElementName<IWordCloudLayouter>(wordCloudLayouter);

            return wordCloudLayouterResult.Then(r =>
            {
                WordCloudLayouter = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingStringSizeProvider(string sizeProvider, Cli cli)
        {
            var sizeProviderResult = cli.GetTypeByCliElementName<IStringSizeProvider>(sizeProvider);

            return sizeProviderResult.Then(r =>
            {
                SizeProvider = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingWordBrushProvider(string brushProvider, Cli cli)
        {
            var brushProviderResult = cli.GetTypeByCliElementName<IWordBrushProvider>(brushProvider);

            return brushProviderResult.Then(r =>
            {
                BrushProvider = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingRectanglePenProvider(string penProvider, Cli cli)
        {
            var penProviderResult = cli.GetTypeByCliElementName<IRectanglePenProvider>(penProvider);

            return penProviderResult.Then(r =>
            {
                PenProvider = r;
                return this;
            });
        }

        public Result<TagCloudConfig> UsingBackgroundBrush(Brush brush)
        {
            Options = Options.WithBackgroundBrush(brush);
            return this;
        }

        public Result<TagCloudConfig> UsingFont(Font font)
        {
            Options = Options.WithFont(font);
            return this;
        }

        public Result<TagCloudConfig> UsingWordBrush(Brush brush)
        {
            Options = Options.WithWordBrush(brush);
            return this;
        }

        public Result<TagCloudConfig> UsingPen(Pen pen)
        {
            Options = Options.WithPen(pen);
            return this;
        }

        public Result<TagCloudConfig> SetSize(string size)
        {
            if (size is null)
                return Result.Fail<TagCloudConfig>("Size can't be null");
            if (!size.Contains('x'))
                return Result.Fail<TagCloudConfig>($"Size {size} has invalid size format. " +
                                                   $"It must be WxH, where W and H is Width and Height");
            try
            {
                var sizes = size.Split('x').Select(int.Parse).ToList();
                Options.ImageSize = new Size(sizes[0], sizes[1]);
            }
            catch (FormatException e)
            {
                return Result.Fail<TagCloudConfig>($"Size {size} has invalid size format. " +
                                                   $"Width and Height have to be positive integers");
            }

            return this;
        }

        public Result<TagCloudConfig> SetImageFormat(ImageFormat imageFormat)
        {
            if (imageFormat is null)
                return Result.Fail<TagCloudConfig>("ImageFormat can't be null");
            ImageFormat = imageFormat;
            return this;
        }

        public Result<None> CreateCloud(string inputFile, string outputFile)
        {
            var bitmap = PrepareContainer(inputFile).Then(c => c.Resolve<Image>());

            if (!bitmap.IsSuccess)
                return Result.Fail<None>(bitmap.Error);
            return bitmap.Then(bmp => bmp.Save(outputFile, ImageFormat));
        }

        private Result<IContainer> PrepareContainer(string inputFile)
        {
            var builder = new ContainerBuilder();
            var textReaderAttempt = WordProvider.Then(func => func(inputFile));

            if (!textReaderAttempt.IsSuccess)
                return Result.Fail<IContainer>(textReaderAttempt.Error);
            var textReader = textReaderAttempt.DefaultIfError(new TxtFileReader(inputFile));

            builder.Register(c => textReader).As<IWordProvider>().SingleInstance();
            builder.RegisterType(WordProcessor).As<IWordProcessor>().SingleInstance();

            builder.Register(c =>
            {
                var words = c.Resolve<IWordProvider>().GetWords();
                words = words.Then(w => c.Resolve<IWordProcessor>().Process(w));
                if (!words.IsSuccess)
                    Console.WriteLine(words.Error);
                return words.DefaultIfError(new List<string>());
            }).As<IEnumerable<string>>();

            builder.RegisterType(CloudLayouter).As<ICloudLayouter>().SingleInstance();
            builder.RegisterType(WordCloudLayouter).As<IWordCloudLayouter>().SingleInstance();
            builder.RegisterType(SizeProvider).As<IStringSizeProvider>().SingleInstance();
            builder.RegisterType(BrushProvider).As<IWordBrushProvider>().SingleInstance();
            builder.RegisterType(PenProvider).As<IRectanglePenProvider>().SingleInstance();

            builder.Register(c => Options).As<DrawingOptions>().SingleInstance();
            builder.Register(c => c.Resolve<DrawingOptions>().Font).As<Font>().SingleInstance();

            builder.RegisterType(WordVisualizer).As<IWordVisualizer>();

            builder.Register(c => c.Resolve<IWordVisualizer>().CreateImageWithWords(
                c.Resolve<IEnumerable<string>>())).As<Image>();
            return Result.Of(() => builder.Build());
        }
    }
}