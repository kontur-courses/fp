using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using TagsCloudGenerator.CloudLayouter;
using TagsCloudGenerator.FileReaders;
using TagsCloudGenerator.Saver;
using TagsCloudGenerator.Tools;
using TagsCloudGenerator.Visualizer;
using TagsCloudGenerator.WordsHandler;
using TagsCloudGenerator.WordsHandler.Converters;
using TagsCloudGenerator.WordsHandler.Filters;

namespace TagsCloudGenerator.Client.Console
{
    internal static class ConsoleDependenciesBuilder
    {
        public static Result<IContainer> BuildContainer(Options options)
        {
            return ReadBoringWords(options.PathToBoringWords)
                .Then(words => ConfigContainer(options, words))
                .Then(container => container.Build())
                .RefineError("Error of configuration");
        }

        private static ContainerBuilder ConfigContainer(Options options, IEnumerable<string> boringWords)
        {
            var builder = new ContainerBuilder();
            var center = new Point(0, 0);

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => typeof(IFileReader).IsAssignableFrom(x) && !x.IsAbstract)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ReaderFactory>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<CyclicColoringAlgorithm>().As<IColoringAlgorithm>().SingleInstance();
            builder.RegisterType<CloudVisualizer>().As<ICloudVisualizer>().SingleInstance();
            builder.RegisterType<ImageSaver>()
                .As<IImageSaver>()
                .WithParameter(new TypedParameter(typeof(string), options.OutputPath))
                .SingleInstance();
            builder.RegisterType<CloudGenerator>().As<ICloudGenerator>().SingleInstance();
            builder.RegisterType<WordHandler>().As<IWordHandler>().SingleInstance();
            builder.RegisterType<WordsParser>().As<IWordsParser>().SingleInstance();

            builder.RegisterInstance<ILayoutPointsGenerator>(new SpiralGenerator(center, 0.5, Math.PI / 16))
                .SingleInstance();

            builder.RegisterType<LowercaseConverter>().As<IConverter>().SingleInstance();
            builder.RegisterType<BoringWordsEjector>()
                .As<IWordsFilter>()
                .WithParameter(new TypedParameter(typeof(IEnumerable<string>), boringWords))
                .SingleInstance();

            builder.RegisterType<InitialFormConverter>().As<IConverter>()
                .WithParameter(new NamedParameter("pathToAff", @"en-GB/index.aff"))
                .WithParameter(new NamedParameter("pathToDictionary", @"en-GB/index.dic"))
                .SingleInstance();

            builder.RegisterInstance(GetImageSettings(options)).AsSelf().SingleInstance();

            builder.RegisterType<CircularCloudLayouter>()
                .WithParameter(new TypedParameter(typeof(Point), center))
                .WithParameter(new TypedParameter(typeof(Size), new Size(6, 10)))
                .As<ICloudLayouter>().SingleInstance();

            return builder;
        }

        private static ImageSettings GetImageSettings(Options options)
        {
            var colors = GetColorsByNames(options.Colors);
            var backgroundColor = Color.FromName(options.BackgroundColor);
            var extension = PathHelper.GetFileExtension(options.OutputPath);
            var format = GetImageFormat(extension);
            var font = new Font(options.Font, options.FontSize);

            return new ImageSettings(options.ImageWidth, options.ImageHeight, backgroundColor, colors, format, font);
        }

        private static List<Color> GetColorsByNames(string names)
        {
            var colors = names.Split(new[] { ", " }, StringSplitOptions.None);

            return colors.Select(Color.FromName).ToList();
        }

        private static ImageFormat GetImageFormat(string extension)
        {
            var info = typeof(ImageFormat)
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(extension, StringComparison.InvariantCultureIgnoreCase));

            return info?.GetValue(info) as ImageFormat;
        }

        private static Result<IEnumerable<string>> ReadBoringWords(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Enumerable.Empty<string>().AsResult();

            return Result
                .Of(() => File.ReadAllText(path))
                .Then(text => text
                    .Split(new[] {" ", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(w => w.Trim()))
                .RefineError($"Couldn't read boring words");
        }
    }
}
