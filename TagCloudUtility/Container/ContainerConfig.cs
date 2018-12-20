using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using Autofac;
using TagCloud.Layouter;
using TagCloud.PointsSequence;
using TagCloud.RectanglePlacer;
using TagCloud.Utility.Models.Tag;
using TagCloud.Utility.Models.Tag.Container;
using TagCloud.Utility.Models.TextReader;
using TagCloud.Utility.Models.WordFilter;
using TagCloud.Utility.Runner;
using TagCloud.Visualizer;
using TagCloud.Visualizer.Drawer;
using TagCloud.Visualizer.Settings;
using TagCloud.Visualizer.Settings.Colorizer;

namespace TagCloud.Utility.Container
{
    public static class ContainerConfig
    {
        public static IContainer StandartContainer => Configure(Options.Standart).Value;
        public static Result<IContainer> Configure(Options options)
        {
            var builder = new ContainerBuilder();

            var brushColor = Result
                .Of(() => ColorTranslator.FromHtml(options.Brush));

            if (!brushColor.IsSuccess)
                return Result.Fail<IContainer>($"Wrong format of brush color: {options.Brush}");

            builder
                .RegisterInstance(new SolidColorizer(brushColor.Value))
                .As<IColorizer>()
                .AsSelf();

            var fc = new InstalledFontCollection();
            if (!fc.Families.Any(ff => options.Font.StartsWith(ff.Name)))
                return Result.Fail<IContainer>($"Wrong format of font: {options.Font}");

            var backgroundColor = Result
                .Of(() => ColorTranslator.FromHtml(options.Color));

            if (!backgroundColor.IsSuccess)
                return Result.Fail<IContainer>($"Wrong format of color: {options.Color}");

            builder
                .RegisterType<DrawSettings>()
                .WithParameter(
                    (pi, c) => pi.ParameterType == typeof(DrawFormat),
                    (pi, c) => options.DrawFormat)
                .WithParameter(
                    (pi, c) => pi.ParameterType == typeof(Font),
                    (pi, c) => new FontConverter().ConvertFromString(options.Font))
                .WithParameter(
                    (pi, c) => pi.ParameterType == typeof(Color),
                    (pi, c) => backgroundColor.Value)
                .As<IDrawSettings>()
                .AsSelf();

            builder
                .RegisterType<Spiral>()
                .As<IPointsSequence>()
                .AsSelf();

            builder
                .RegisterType<CenterRectanglePlacer>()
                .As<IRectanglePlacer>()
                .AsSelf();

            var sizeArr = options.Size.Split('x');
            if (sizeArr.Length != 2 || !int.TryParse(sizeArr[0], out var x) || !int.TryParse(sizeArr[1], out var y))
                return Result.Fail<IContainer>($"Expected format of size INTxINT, but was {options.Size}");

            builder
                .Register(_ => new Size(x, y))
                .As<Size>();

            builder
                .RegisterType<CircularCloudLayouter>()
                .As<ICloudLayouter>()
                .AsSelf();

            builder
                .RegisterType<CloudDrawer>()
                .As<ICloudDrawer>()
                .AsSelf();

            builder
                .RegisterType<CloudVisualizer>()
                .As<ICloudVisualizer>()
                .AsSelf();

            builder
                .RegisterType<TagContainerReader>()
                .As<ITagContainerReader>()
                .Named<ITagContainerReader>(".txt")
                .Named<ITagContainerReader>(".ini")
                .AsSelf();

            if (options.PathToTags != null)
            {
                var tagsExtension = Helper.GetExtension(options.PathToTags);
                if (tagsExtension != ".txt" && tagsExtension != ".ini")
                    return Result.Fail<IContainer>($"Not supported tag file extension: {tagsExtension}");
                var tagContainer = Result
                    .Of(() => new TagContainerReader().ReadTagsContainer(Helper.GetPath(options.PathToTags)))
                    .RefineError("Error while reading tag groups");
                if (!tagContainer.IsSuccess)
                    return Result.Fail<IContainer>(tagContainer.Error);
                builder.RegisterInstance(tagContainer.Value)
                    .As<ITagContainer>()
                    .AsSelf();
            }
            else
            {
                builder
                    .RegisterInstance(
                        new TagContainer
                        {
                            {"Big", new FrequencyGroup(0.9, 1), 35},
                            {"Average", new FrequencyGroup(0.6, 0.9), 25},
                            {"Small", new FrequencyGroup(0, 0.6), 15}
                        })
                    .As<ITagContainer>()
                    .AsSelf();
            }

            builder
                .RegisterType<TagReader>()
                .As<ITagReader>()
                .AsSelf();

            builder
                .RegisterType<TxtReader>()
                .As<ITextReader>()
                .Named<ITextReader>(".txt")
                .Named<ITextReader>(".ini")
                .AsSelf();

            if (options.PathToStopWords != null)
            {
                var stopWordsExtension = Helper.GetExtension(options.PathToStopWords);
                if (stopWordsExtension != ".txt" && stopWordsExtension != ".ini")
                    return Result.Fail<IContainer>($"Not supported stop words file extension: {stopWordsExtension}");
                var reader = Result
                    .Of(() => new TxtReader().ReadToEnd(Helper.GetPath(options.PathToStopWords)))
                    .RefineError("Error while reading stop words");
                if (!reader.IsSuccess)
                    return Result.Fail<IContainer>(reader.Error);
                builder
                    .RegisterType<WordFilter>()
                    .WithParameter(
                        (pi, c) => pi.ParameterType == typeof(IEnumerable<string>),
                        (pi, c) => reader.Value)
                    .As<IWordFilter>()
                    .AsSelf();
            }
            else
            {
                builder
                    .RegisterType<WordFilter>()
                    .As<IWordFilter>()
                    .AsSelf();
            }

            var imageFormat = Helper.GetImageFormat(options.PathToPicture);
            if (!imageFormat.IsSuccess)
                return Result.Fail<IContainer>(imageFormat.Error);
            builder
                .RegisterType<TagCloudRunner>()
                .WithParameter(
                    (pi, c) => pi.Name == "pathToWords",
                    (pi, c) => options.PathToWords)
                .WithParameter(
                    (pi, c) => pi.Name == "pathToPicture",
                    (pi, c) => options.PathToPicture)
                .WithParameter(
                    (pi, c) => pi.ParameterType == typeof(ImageFormat),
                    (pi, c) => imageFormat.Value)
                .As<ITagCloudRunner>()
                .AsSelf();

            return Result
                .Of(() => builder.Build())
                .RefineError("Error while configuring");
        }
    }
}