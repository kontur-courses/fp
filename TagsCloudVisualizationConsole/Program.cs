using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.MorphAnalyzer;
using TagsCloudVisualization.Parsing;
using TagsCloudVisualization.Reading;
using TagsCloudVisualization.Words;
using TagsCloudVisualizationConsole;


try
{
    var appOptions = new ArgsOptions();

    if (!File.Exists("appsettings.json"))
        Console.WriteLine("File with configuration settings not exits");
    else
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddCommandLine(args)
            .Build();
        appOptions = configuration.Get<ArgsOptions>();
    }

    var validateResult = AppOptionsValidator.ValidatePathsInOptions(appOptions);
    if (!validateResult.IsSuccess)
    {
        Console.WriteLine(validateResult.Error);
        Console.Read();
        return;
    }

    var optionsResult = VisualizationOptionsConverter.ConvertOptions(appOptions!);

    if (!optionsResult.IsSuccess)
    {
        Console.WriteLine(optionsResult.Error);
        Console.Read();
        return;
    }

    var options = optionsResult.GetValueOrThrow();

    var container = new ServiceCollection()
        .AddTransient<ITextReader>(r => new PlainTextFromFileReader(appOptions!.PathToTextFile))
        .AddSingleton<ITextParser, SingleColumnTextParser>()
        .AddSingleton<IWordsLoader, CustomWordsLoader>()
        .AddTransient<IWordsFilter, CustomWordsFilter>()
        .AddTransient<IMorphAnalyzer>(r => new MyStemMorphAnalyzer(appOptions!.DirectoryToMyStemProgram))
        .AddSingleton<IWordsSizeCalculator, CustomWordSizeCalculator>()
        .AddSingleton<ICloudLayouter, CircularCloudLayouter>()
        .AddSingleton<ISpiralFormula, ArithmeticSpiral>()
        .AddTransient<TagCloudVisualizations>()
        .BuildServiceProvider();

    var visualizations = container.GetRequiredService<TagCloudVisualizations>();

    var bitmap = visualizations.DrawCloud(options);

    var imageFormatResult = AppOptionsValidator.GetImageFormat(appOptions!.FileExtension);
    if (!imageFormatResult.IsSuccess)
    {
        Console.WriteLine(imageFormatResult.Error);
        return;
    }

    if (bitmap.IsSuccess)
        bitmap.GetValueOrThrow().Save(Path.Combine(appOptions.DirectoryToSaveFile, string.Concat(appOptions.SaveFileName, ".", appOptions.FileExtension.ToLower())), imageFormatResult.GetValueOrThrow());
    else
    {
        Console.WriteLine(bitmap.Error);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.Read();