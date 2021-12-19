using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using TagCloud.Analyzers;
using TagCloud.Creators;
using TagCloud.Layouters;
using TagCloud.Readers;
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

            builder.RegisterInstance(drawingSettings).AsSelf().SingleInstance();

            builder.RegisterType<FileReaderFactory>().As<IFileReaderFactory>().SingleInstance();
            builder.RegisterType<TextAnalyzer>().As<ITextAnalyzer>().SingleInstance();
            builder.RegisterType<FrequencyAnalyzer>().As<IFrequencyAnalyzer>().SingleInstance();
            
            builder.RegisterType<TagCreator>().As<ITagCreator>()
                .WithParameter(new TypedParameter(typeof(Font), drawingSettings.Font))
                .SingleInstance();

            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>()
                .WithParameter(new TypedParameter(typeof(Point), center))
                .SingleInstance();

            builder.RegisterType<CloudVisualizer>().As<IVisualizer>().SingleInstance();
            builder.RegisterType<BitmapWriter>().As<IFileWriter>().SingleInstance();
            builder.RegisterType<WordsToLowerConverter>().As<IWordConverter>().SingleInstance();
            builder.RegisterType<BoringWordsFilter>()
                .As<IWordFilter>()
                .As<BoringWordsFilter>()
                .SingleInstance();
            builder.RegisterType<TagColoringFactory>().As<ITagColoringFactory>().SingleInstance();

            builder.RegisterType<ConsoleUI>().As<IUserInterface>().SingleInstance();
            return builder.Build();
        }

        private static DrawingSettings GetDrawingSettings(Options options)
        {
            var backgroundColor = Color.FromName(options.BackgroundColor);
            var penColors = ParseColors(options.WordColors);
            var font = new Font(options.FontName, options.FontSize);
            
            return new DrawingSettings(penColors,
                backgroundColor,
                font,
                options.Width,
                options.Height,
                options.TagColoring);
        }

        private static IEnumerable<Color> ParseColors(IEnumerable<string> colors)
        {
            return colors.Select(Color.FromName);
        }
    }
}
