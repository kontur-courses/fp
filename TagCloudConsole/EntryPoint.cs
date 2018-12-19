using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Autofac;
using CommandLine;
using TagsCloud;

namespace TagCloudConsole
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleErrors);
        }

        private static void Run(Options options)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TagCloudLayouter>().As<ITagCloudLayouter>();
            containerBuilder.RegisterType<TagCloudRender>().AsSelf();
            var boringWords = new LowerWords(new WordsFromFile(options.BoringWordsPath)).ToLower();
            var words = new LowerWords(GetWordsFromFile(options)).ToLower();
            containerBuilder.RegisterInstance(
                new BoringWordsFilter(
                    boringWords,
                    words
                )
            ).As<IBoringWordsCollection>();
            containerBuilder.RegisterInstance(
                    new CoordinatesAtImage(
                        new Size(
                            options.Height, options.Width)))
                .AsSelf();
            containerBuilder.RegisterType<TagCloudLayouter>().AsSelf();
            containerBuilder.RegisterType<FrequencyCollection>().As<IFrequencyCollection>();
            var center = new Point(0, 0);
            var step = 0.01;
            var width = 0.1;
            containerBuilder.RegisterInstance(
                    new CircularCloudLayouter(
                        center, new CircularSpiral(center, width, step)))
                .As<ICloudLayouter>();
            var imageSettings = CheckImageSettings(options);
            containerBuilder.RegisterInstance(new Picture(imageSettings)).As<IGraphics>();

            using (var container = containerBuilder.Build())
            {
                var render = container.Resolve<TagCloudRender>();
                render.Render().OnFail(Console.WriteLine);
            }
        }

        private static Result<ImageSettings> CheckImageSettings(Options options)
        {
            var font = new Font(options.FontFamily, 20);
            if (options.FontFamily != font.Name)
                return Result.Fail<ImageSettings>("Incorrect font family");

            if (options.Width <= 0)
                return Result.Fail<ImageSettings>("Incorrect width: was zero or negative");

            if (options.Height <= 0)
                return Result.Fail<ImageSettings>("Incorrect height: was zero or negative");

            return GetColor(options.Color).Then(color =>
            {
                return GetImageFormat(options.Format)
                    .Then(imageFormat =>
                    {
                        return new ImageSettings(new Size(options.Height, options.Width),
                            new FontFamily(options.FontFamily),
                            color,
                            imageFormat,
                            options.Name);
                    });
            });
        }

        private static Result<Color> GetColor(string colorName)
        {
            var color = Color.FromName(colorName);
            if (color.IsKnownColor)
                return color;
            return Result.Fail<Color>("Incorrect color");
        }

        private static IWordCollection GetWordsFromFile(Options options)
        {
            if (options.IsDocx)
                return new WordsFromMicrosoftWord(options.TextPath);
            return new WordsFromFile(options.TextPath);
        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine(string.Join(Environment.NewLine, errors));
        }

        private static Result<ImageFormat> GetImageFormat(string format)
        {
            var lowerFormat = format.ToLower();
            switch (lowerFormat)
            {
                case "png":
                    return ImageFormat.Png;
                case "bmp":
                    return ImageFormat.Bmp;
                case "jpeg":
                    return ImageFormat.Jpeg;
            }

            return Result.Fail<ImageFormat>(
                $"Incorrect image format. Expected one of: png, jpeg, bmp, but was {format}");
        }
    }
}