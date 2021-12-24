using System.Drawing;
using Autofac;
using TagCloud;
using TagCloud.CloudLayouter;
using TagCloud.PointGenerator;
using TagCloud.Templates;
using TagCloud.Templates.Colors;
using TagCloud.TextHandlers;
using TagCloud.TextHandlers.Converters;
using TagCloud.TextHandlers.Filters;
using TagCloud.TextHandlers.Parser;
using TagCloudApp.Configurations;

namespace TagCloudApp;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder GetDefaultBuild(this ContainerBuilder builder, Configuration configuration)
    {
        builder.Register(_ => configuration).SingleInstance();
        RegisterTextHandlers(builder);
        RegisterCloudLayouter(builder);
        RegisterTemplateHandlers(builder);
        return builder;
    }

    private static void RegisterTextHandlers(ContainerBuilder builder)
    {
        builder.RegisterType<WordsReader>().As<IReader>();
        builder.RegisterType<WordsFromTextParser>().As<ITextParser>();
        builder.RegisterType<BoringWordsFilter>().As<IFilter>();
        builder.RegisterType<TextFilter>().As<ITextFilter>();
        builder.RegisterType<ToLowerConverter>().As<IConverter>();
        builder.RegisterType<ToInfinitiveConverter>().As<IConverter>();
        builder.RegisterType<TextFilter>().As<ITextFilter>();
        builder.RegisterType<ConvertersPool>().As<IConvertersPool>();
        builder.RegisterType<FontSizeByCountCalculator>().AsSelf();
    }

    private static void RegisterTemplateHandlers(ContainerBuilder builder)
    {
        builder.Register(c => c.Resolve<Configuration>().FontFamily).As<FontFamily>();
        builder.RegisterType<Template>().As<ITemplate>();
        builder.Register(c => c.Resolve<Configuration>().ToTemplateConfiguration()).As<TemplateConfiguration>();
        builder.RegisterType<TemplateCreator>().As<ITemplateCreator>();
        builder.RegisterType<WordParameter>().AsSelf();
        builder.Register(c => c.Resolve<Configuration>().ColorGenerator).As<IColorGenerator>();
        builder.Register(_ => new FontSizeByCountCalculator(Configuration.MinFontSize, Configuration.MaxFontSize))
            .As<IFontSizeCalculator>();
        builder.RegisterType<Visualizer>().As<IVisualizer>();
    }

    private static void RegisterCloudLayouter(ContainerBuilder builder)
    {
        builder.RegisterType<Cache>().As<ICache>();
        builder.Register(c => c.Resolve<Configuration>().PointGenerator).As<IPointGenerator>();
        builder.RegisterType<CloudLayouter>().AsSelf().As<ICloudLayouter>();
    }
}