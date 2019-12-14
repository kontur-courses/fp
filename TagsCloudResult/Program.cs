using Autofac;
using TagsCloudResult.ApplicationRunning;
using TagsCloudResult.ApplicationRunning.Commands;
using TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands;
using TagsCloudResult.ApplicationRunning.UIApp;
using TagsCloudResult.CloudLayouters;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.CloudVisualizers.BitmapMakers;
using TagsCloudResult.CloudVisualizers.ImageSaving;
using TagsCloudResult.TextParsing;
using TagsCloudResult.TextParsing.CloudParsing;
using TagsCloudResult.TextParsing.CloudParsing.ParsingRules;
using TagsCloudResult.TextParsing.FileWordsParsers;

namespace TagsCloudResult
{
    public static class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TagsCloudMaker>().AsSelf().SingleInstance();
            builder.RegisterType<UIAppRunner>().As<IAppRunner>();
            builder.RegisterType<TagsCloud>().AsSelf().SingleInstance();
            builder.RegisterType<CloudWordsParser>().As<ICloudWordsParser>();
            builder.RegisterType<CloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<CloudVisualizer>().As<ICloudVisualizer>();
            builder.RegisterType<ImageSaver>().As<IImageSaver>();
            builder.RegisterType<TxtWordParser>().As<IFileWordsParser>();
            builder.RegisterType<DefaultParsingRule>().As<ICloudWordParsingRule>();
            builder.RegisterType<DefaultBitmapMaker>().As<IBitmapMaker>();
            builder.RegisterType<SettingsManager>().AsSelf().SingleInstance();
            builder.RegisterType<CommandsExecutor>().AsSelf().SingleInstance();
            builder.RegisterType<ParseCommand>().As<IConsoleCommand>();
            builder.RegisterType<GenerateCloudCommand>().As<IConsoleCommand>();
            builder.RegisterType<VisualizeCommand>().As<IConsoleCommand>();
            builder.RegisterType<SaveCommand>().As<IConsoleCommand>();
            builder
                .Register(c => c.Resolve<SettingsManager>().GetLayouterSettings())
                .As<CloudLayouterSettings>();
            builder
                .Register(c => c.Resolve<SettingsManager>().GetWordsParserSettings())
                .As<CloudWordsParserSettings>();
            builder
                .Register(c => c.Resolve<SettingsManager>().GetVisualizerSettings())
                .As<CloudVisualizerSettings>();
            builder
                .Register(c => c.Resolve<SettingsManager>().GetImageSaverSettings())
                .As<ImageSaverSettings>();
            var container = builder.Build();
            container.Resolve<TagsCloudMaker>();
        }
    }
}