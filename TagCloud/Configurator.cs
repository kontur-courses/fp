using Autofac;
using CommandLine;
using ResultOf;
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
    public static IAppSettings Parse(string[] args)
    {
        var settings = Parser.Default.ParseArguments<Settings>(args);

        if (settings as Parsed<Settings> == null)
            Environment.Exit(-1);

        SettingsValidator.SizeIsValid(settings.Value)
            .Then(SettingsValidator.FontIsInstalled)
            .OnFail(e =>
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            });

        return settings.Value;
    }

    public static ContainerBuilder ConfigureBuilder(IAppSettings settings)
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<TxtReader>().As<IFileReader>();
        builder.RegisterType<DocReader>().As<IFileReader>();
        builder.RegisterType<ImageSaver>().As<ISaver>();
        builder.RegisterType<CloudDrawer>().As<IDrawer>();
        builder.RegisterType<WordRankerByFrequency>().As<IWordRanker>();
        builder.RegisterType<DefaultPreprocessor>().As<IPreprocessor>();
        builder.RegisterType<SpiralGenerator>().As<IPointGenerator>();
        builder.RegisterType<CirclesGenerator>().As<IPointGenerator>();
        builder.RegisterType<RandomPalette>().As<IPalette>();
        builder.RegisterType<CustomPalette>().As<IPalette>();

        builder.RegisterType<ConsoleUI>().As<IUserInterface>();

        builder.Register(c => new WordFilter().UsingFilter((word) => word.Length > 3)).As<IFilter>();
        builder.Register(c => new FileReaderProvider(c.Resolve<IEnumerable<IFileReader>>())).As<IFileReaderProvider>();
        builder.Register(c => new PointGeneratorProvider(c.Resolve<IEnumerable<IPointGenerator>>()))
            .As<IPointGeneratorProvider>();
        builder.Register(c => new PaletteProvider(c.Resolve<IEnumerable<IPalette>>())).As<IPaletteProvider>();

        builder.Register(c => settings).As<IAppSettings>();

        return builder;
    }
}