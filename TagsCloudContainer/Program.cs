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

        var container = new ServiceCollection()
            .AddTransient<IConverter, FileToDictionaryConverter>()
            .AddSingleton<IDocParser, BudgetDocParser>()
            .AddTransient<IWordsFilter, WordsFilter>()
            .AddSingleton<ISpiralDrawer, SpiralDrawer>()
            .AddSingleton<IImageFormatProvider, ImageFormatProvider>()
            .AddSingleton<IOptionsValidator, CustomOptionsValidator>()
            .AddSingleton<IWordSizeCalculator, WordSizeCalculator>()
            .AddTransient<IDrawer, CloudDrawer>()
            .BuildServiceProvider();

        var validator = container.GetService<IOptionsValidator>();
        var validationResult = validator.ValidateOptions(options);
        if (!validationResult.IsSuccess)
        {
            Console.WriteLine(validationResult.Error);
            return;
        }

        var drawer = container.GetService<IDrawer>();

        var drawingResult = drawer.DrawCloud(options);
        Console.WriteLine(drawingResult.IsSuccess ? drawingResult.GetValueOrThrow() : drawingResult.Error);
    }
}