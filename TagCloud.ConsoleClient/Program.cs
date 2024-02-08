using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var options = Parser.Default.ParseArguments<Options>(args);

        if (options.Errors.Any())
            return;

        IConfiguration config = 
            Result.Of(() => GetConfiguration(PathFinderHelper.GetPathToFile("defaultSettings.json")))
                  .ReplaceError(errorMessage => "Can't load default settings \n" + errorMessage)
                  .GetValueOrThrow();

        var appOptions = AppOptionsCreator.CreateOptions(options.Value, config);

        var provider = GetServiceProvider(appOptions);

        var inp = options.Value.InputTextPath;
        var outp = options.Value.OutputImagePath;

        var textLoader = provider.GetService<ITextLoader>();
        var tagCloud = provider.GetService<ITagCloud>();
        var imageStorage = provider.GetService<IImageStorage>();

        var result = textLoader.Load(inp)
                      .Then(text => tagCloud.CreateCloud(text))
                      .Then(image => imageStorage.Save(image, outp))
                      .OnFail(Console.WriteLine);
    }

    private static IConfiguration GetConfiguration(string pathToFile)
    {
        return new ConfigurationBuilder()
            .AddJsonFile(pathToFile)
            .Build();
    }

    private static ServiceProvider GetServiceProvider(AppOptions options)
    {
        var builder = new ServiceCollection();
        builder.AddClient();
        builder.AddDomain(options.DomainOptions);
        builder.AddInfrastructure();
        return builder.BuildServiceProvider();
    }
}