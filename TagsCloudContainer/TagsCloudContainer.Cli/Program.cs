using CSharpFunctionalExtensions;
using DryIoc;
using McMaster.Extensions.CommandLineUtils;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Cli;

[Command(Name = "draw")]
[HelpOption("-?")]
// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    [Option(CommandOptionType.SingleOrNoValue)]
    private string JsonSettingsFilename { get; } =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tag-cloud-settings.json");

    [Option(CommandOptionType.SingleOrNoValue)]
    private string WordsFileName { get; } =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tag-cloud-words.txt");

    public static int Main(string[] args)
    {
        return CommandLineApplication.Execute<Program>(args);
    }

    /// <summary>
    ///     Using api of McMaster.Extensions.CommandLineUtils
    /// </summary>
    /// <param name="app"></param>
    /// <returns>status code of program execution: 1 if settings isn't provided, in case of success execution returns 0</returns>
    // ReSharper disable once UnusedMember.Local
    private int OnExecute(CommandLineApplication app)
    {
        if (string.IsNullOrEmpty(JsonSettingsFilename)
            || string.IsNullOrWhiteSpace(WordsFileName))
        {
            app.ShowHelp();
            return 1;
        }

        var result = Result.Success(GetServices())
            .BindTry(
                tuple => Result.Success((tuple.drawer, tuple.selector, allWords: File.ReadAllLines(WordsFileName))))
            .Bind(tuple => (tuple.drawer, wordsResult: tuple.selector.RecognizeFunnyCloudWords(tuple.allWords)))
            .Ensure(tuple => tuple.wordsResult.IsSuccess, err => err.wordsResult.Error)
            .Bind(tuple => (tuple.drawer, words: tuple.wordsResult.Value))
            .Bind(tuple => tuple.drawer.Draw(tuple.words));
        if (!result.IsFailure) return 0;
        Console.WriteLine("Errors: ");
        Console.WriteLine(result.Error);
        return 1;
    }

    private (IFunnyWordsSelector selector, MultiDrawer drawer) GetServices()
    {
        var container = ContainerHelper.RegisterDefaultSingletonContainer();
        container.Register<IGraphicsProvider, CliGraphicsProvider>(Reuse.Singleton);
        container.RegisterDelegate(r =>
            (CliGraphicsProviderSettings)r.Resolve<Settings>().GraphicsProviderSettings);
        var jsonSettingsFactory = new JsonSettingsFactory(JsonSettingsFilename);
        container.RegisterDelegate<ISettingsFactory>(_ => jsonSettingsFactory,
            Reuse.Singleton);

        var selector = container.Resolve<IFunnyWordsSelector>();
        var drawer = container.Resolve<MultiDrawer>();
        return (selector, drawer);
    }
}