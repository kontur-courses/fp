using System.Drawing;
using Autofac;
using CommandLine;
using TagCloud.AppSettings;
using TagCloud.Drawer;
using TagCloud.FileReader;
using TagCloud.FileSaver;
using TagCloud.Filter;
using TagCloud.PointGenerator;
using TagCloud.UserInterface;
using TagCloud.WordRanker;
using TagCloud.WordsPreprocessor;

namespace TagCloud;

public class Configurator
{
    public static IAppSettings Parse(string[] args, ContainerBuilder builder)
    {
        var sets = Parser.Default.ParseArguments<Settings>(args);
        if (sets as Parsed<Settings> == null)
            Environment.Exit(-1);
        var settings = sets.WithParsed(o =>
        {
            if (o.UseRandomPalette)
                builder.RegisterType<RandomPalette>().As<IPalette>();
            else
                builder.Register(p =>
                    new CustomPalette(Color.FromName(o.ForegroundColor), Color.FromName(o.BackgroundColor)));
        });

        return settings.Value;
    }

    public static ContainerBuilder BuildWithSettings(IAppSettings settings, ContainerBuilder builder)
    {
        builder.RegisterType<TxtReader>().As<IFileReader>();
        builder.RegisterType<DocReader>().As<IFileReader>();
        builder.RegisterType<ImageSaver>().As<ISaver>();
        builder.RegisterType<CloudDrawer>().As<IDrawer>();
        builder.RegisterType<WordRankerByFrequency>().As<IWordRanker>();
        builder.RegisterType<DefaultPreprocessor>().As<IPreprocessor>();
        builder.RegisterType<SpiralGenerator>().As<IPointGenerator>();
        builder.RegisterType<CirclesGenerator>().As<IPointGenerator>();

        builder.RegisterType<ConsoleUI>().As<IUserInterface>();

        builder.Register(c => new WordFilter().UsingFilter((word) => word.Length > 3)).As<IFilter>();
        builder.Register(c => new FileReaderProvider(c.Resolve<IEnumerable<IFileReader>>())).As<IFileReaderProvider>();
        builder.Register(c => new PointGeneratorProvider(c.Resolve<IEnumerable<IPointGenerator>>()))
            .As<IPointGeneratorProvider>();

        builder.Register(c => settings).As<IAppSettings>();

        return builder;
    }
}