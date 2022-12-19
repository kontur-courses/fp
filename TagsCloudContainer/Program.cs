using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CloudLayout;
using CloudLayout.Interfaces;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", optional: false)
            .AddCommandLine(args)
            .Build();

        var options = configuration.Get<CustomOptions>();
        var validatorResult = CustomOptionsValidator.ValidateOptions(options);
        if (!validatorResult)
        {
            Console.WriteLine(validatorResult.Exception!.Message);
            return;
        }

        var container = new ServiceCollection()
            .AddTransient<IConverter, FileToDictionaryConverter>()
            .AddSingleton<IDocParser, BudgetDocParser>()
            .AddTransient<IWordsFilter, WordsFilter>()
            .AddSingleton<ISpiralDrawer, SpiralDrawer>()
            .AddSingleton<IWordSizeCalculator, WordSizeCalculator>()
            .AddTransient<CloudDrawer>()
            .BuildServiceProvider();

        var drawer = ActivatorUtilities.CreateInstance<CloudDrawer>(container);

        var result = drawer.DrawCloud(validatorResult);
        Console.WriteLine(result ? result.Value : result.Exception!.Message);
    }
}