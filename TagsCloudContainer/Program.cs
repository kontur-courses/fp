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
        if (!validatorResult.IsSuccess)
        {
            Console.WriteLine(validatorResult.Error);
            return;
        }

        var container = new ServiceCollection()
            .AddTransient<IConverter, FileToDictionaryConverter>()
            .AddSingleton<IDocParser, BudgetDocParser>()
            .AddTransient<IWordsFilter, WordsFilter>()
            .AddSingleton<ISpiralDrawer, SpiralDrawer>()
            .AddSingleton<IWordSizeCalculator, WordSizeCalculator>()
            .AddTransient<IDrawer,CloudDrawer>()
            .BuildServiceProvider();

        var drawer = container.GetService<IDrawer>();

        var result = drawer.DrawCloud(options);
        Console.WriteLine(result.IsSuccess? result.GetValueOrThrow():result.Error);
    }
}