using System.Drawing;
using Autofac;
using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.Processor;
using TagCloud.Provider;
using TagCloud.Readers;
using TagCloud.Settings;
using TagCloud.Visualizers;
using TagCloud.Writers;

namespace TagCloud.UI.Console
{
    public static class DependencyConfigurator
    {
        public static IContainer GetConfiguredContainer(Options options)
        {
            var builder = new ContainerBuilder();
            var drawingSettings = GetDrawingSettings(options);
            var center = new Point(drawingSettings.Width / 2, drawingSettings.Height / 2);

            builder.RegisterInstance(drawingSettings)
                .As<IProcessorSettings>()
                .As<IDrawingSettings>()
                .As<ITagCreatorSettings>().SingleInstance();

            builder.RegisterType<FileReaderFactory>().As<IFileReaderFactory>().SingleInstance();
            builder.RegisterType<TextAnalyzer>().As<ITextAnalyzer>().SingleInstance();
            builder.RegisterType<FrequencyAnalyzer>().As<IFrequencyAnalyzer>().SingleInstance();
            builder.RegisterType<WordProvider>().As<IWordProvider>().SingleInstance();

            builder.RegisterType<TagCreator>().As<ITagCreator>()
                .SingleInstance();

            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>()
                .WithParameter(new TypedParameter(typeof(Point), center))
                .SingleInstance();

            builder.RegisterType<CloudVisualizer>().As<IVisualizer>().SingleInstance();
            builder.RegisterType<BitmapWriter>().As<IFileWriter>().SingleInstance();
            
            builder.RegisterType<WordsToLowerConverter>().As<IWordConverter>().SingleInstance();
            builder.RegisterType<BoringWordsFilter>().As<IWordFilter>().SingleInstance();
            
            builder.RegisterType<TagColoringFactory>().As<ITagColoringFactory>().SingleInstance();

            builder.RegisterType<TagCloudProcessor>().As<ITagCloudProcessor>().SingleInstance();
            return builder.Build();
        }

        private static ProcessorSettings GetDrawingSettings(Options options)
        {
            return new ProcessorSettings(options.WordColors,
                options.BackgroundColor,
                options.FontName,
                options.FontSize,
                options.Width,
                options.Height,
                options.TagColoring,
                options.ExcludedWordsFile,
                options.InputFilename,
                options.OutputFilename,
                options.TargetDirectory);
        }
    }
}