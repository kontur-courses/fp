using Autofac;
using TagCloud.Infrastructure.FileReader;
using TagCloud.Infrastructure.Filter;
using TagCloud.Infrastructure.Layouter;
using TagCloud.Infrastructure.Lemmatizer;
using TagCloud.Infrastructure.Painter;
using TagCloud.Infrastructure.Pipeline;
using TagCloud.Infrastructure.Pipeline.Common;
using TagCloud.Infrastructure.Saver;
using TagCloud.Infrastructure.Weigher;

namespace TagCloud;

internal static class Configurator
{
    public static ContainerBuilder ConfigureConsoleClient(this ContainerBuilder builder, IAppSettings appSettings)
    {
        builder.RegisterType<ImageProcessor>().As<IImagePipeline>();
        builder.RegisterType<RandomPalette>().As<IPalette>();
        builder.RegisterType<Painter>().As<IPainter>();
        builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>().AsSelf();
        builder.RegisterType<ImageSaver>().As<IImageSaver>().SingleInstance();
        builder.RegisterType<WordWeigher>().As<IWordWeigher>().SingleInstance();
        builder.RegisterType<RussianLemmatizer>().As<ILemmatizer>().SingleInstance();
        builder.RegisterType<DocFileReader>().As<IFileReader>().SingleInstance();
        builder.RegisterType<PlainTextFileReader>().As<IFileReader>().AsSelf().SingleInstance();

        builder.Register(_ => appSettings).AsImplementedInterfaces().SingleInstance();
        builder.Register(_ => new Filter().AddCondition(AuxiliaryPartOfSpechCondition.Filter))
            .As<IFilter>().SingleInstance();
        builder.Register(c => new FileReaderFactory(c.Resolve<IEnumerable<IFileReader>>()))
            .As<IFileReaderFactory>().SingleInstance();
        builder.Register(c => new CloudLayouterFactory(c.Resolve<IEnumerable<ICloudLayouter>>(), c.Resolve<CircularCloudLayouter>()))
            .As<ICloudLayouterFactory>();

        return builder;
    }
}