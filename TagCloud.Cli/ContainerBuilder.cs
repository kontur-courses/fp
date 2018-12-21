using System;
using System.IO;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TagCloud;
using TagCloud.Interfaces;
using TagCloud.Layouter;
using TagCloud.Visualizer;

namespace TagCloudCreator
{
    public static class ContainerBuilder
    {
        public static Result<IWindsorContainer> ConfigureContainer(Configuration configuration)
        {
            var layouters = TypesCollector.CollectLayouters();
            var colorSchemes = TypesCollector.CollectColorSchemes();
            var fontSchemes = TypesCollector.CollectFontSchemes();
            var sizeSchemes = TypesCollector.CollectSizeSchemes();
            var container = new WindsorContainer();

            return Result.Of(() => container.Register(
                GetRegistration<IWordFilter, WordFilter>()
                    .WithArgument("path", configuration.StopWordsFile)
                    .WithArgument("ignoreBoring", configuration.IgnoreBoring),
                GetRegistration<IVisualizer, Visualizer>()
                    .WithArgument("backgroundColor", configuration.BackgroundColor)
                    .WithArgument("imageSize", configuration.ImageSize),
                GetRegistration<IWordProcessor, InfinitiveCastProcessor>()
                    .WithArgument("affixFileData",
                        ReadEmbeddedFile("TagCloudCreator.Dictionaries.Russian.ru_RU.aff").GetValueOrThrow())
                    .WithArgument("dictionaryFileData",
                        ReadEmbeddedFile("TagCloudCreator.Dictionaries.Russian.ru_RU.dic").GetValueOrThrow()),
                GetRegistration<IStatisticsCollector, StatisticsCollector>(),
                GetRegistration<ICloudLayouter, SpiralCloudLayouter>(),
                GetRegistration<IImageSaver, ImageSaver>(),
                GetRegistration<IFileReader, FileReader>(),
                GetRegistration<Point, Point>(),
                GetRegistration<Application, Application>(),
                GetRegistration(typeof(ISpiral), layouters[configuration.LayouterType]),
                GetRegistration(typeof(IColorScheme), colorSchemes[configuration.ColorScheme]),
                GetRegistration(typeof(IFontScheme), fontSchemes[configuration.FontScheme]),
                GetRegistration(typeof(ISizeScheme), sizeSchemes[configuration.SizeScheme])));
        }

        public static Result<byte[]> ReadEmbeddedFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return Result.Of(() => assembly.GetManifestResourceStream(resourceName))
                .Then(stream =>
                {
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();
                    sr.Dispose();
                    return content;
                })
                .Then(content => Encoding.ASCII.GetBytes(content))
                .ReplaceError(error => "Cannot open embedded resource: " + resourceName);
        }

        public static ComponentRegistration<object> GetRegistration(Type elementFor, Type by)
        {
            return Component.For(elementFor).ImplementedBy(by);
        }

        public static ComponentRegistration<TFor> GetRegistration<TFor, TBy>()
            where TFor : class
            where TBy : TFor
        {
            return Component.For<TFor>().ImplementedBy<TBy>();
        }
    }
}