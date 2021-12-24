using System.Drawing;
using Autofac;
using TagCloud.CloudLayouter;
using TagCloud.Configuration;
using TagCloud.PointGenerator;
using TagCloud.Templates;
using TagCloud.Templates.Colors;
using TagCloud.TextHandlers;
using TagCloud.TextHandlers.Converters;
using TagCloud.TextHandlers.Filters;
using TagCloud.TextHandlers.Parser;

namespace TagCloud;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder GetDefaultBuild(this ContainerBuilder builder,
        Configuration.Configuration configuration)
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

    private static void RegisterCloudLayouter(ContainerBuilder builder)
    {
        builder.RegisterType<Cache>().As<ICache>();
        builder.Register(c => c.Resolve<Configuration.Configuration>().PointGenerator).As<IPointGenerator>();
        builder.RegisterType<CloudLayouter.CloudLayouter>().AsSelf().As<ICloudLayouter>();
    }

    private static void RegisterTemplateHandlers(ContainerBuilder builder)
    {
        builder.Register(c => c.Resolve<Configuration.Configuration>().FontFamily).As<FontFamily>();
        builder.Register(c => c.Resolve<Configuration.Configuration>().ToTemplateConfiguration()).As<TemplateConfiguration>();
        builder.Register(c => c.Resolve<Configuration.Configuration>().ColorGenerator).As<IColorGenerator>();
        builder.Register(_ =>
            new FontSizeByCountCalculator(Configuration.Configuration.MinFontSize,
                Configuration.Configuration.MaxFontSize)).As<IFontSizeCalculator>();
        builder.RegisterType<Template>().As<ITemplate>();
        builder.RegisterType<TemplateCreator>().As<ITemplateCreator>();
        builder.RegisterType<WordParameter>().AsSelf();
        builder.RegisterType<Visualizer>().As<IVisualizer>();
    }
}