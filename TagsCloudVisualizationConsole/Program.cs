using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.MorphAnalyzer;
using TagsCloudVisualization.Parsing;
using TagsCloudVisualization.Reading;
using TagsCloudVisualization.Words;
using TagsCloudVisualizationConsole;


Result<ArgsOptions> LoadAppOptions(string[] strings)
{
    if (!File.Exists("appsettings.json"))
        return Result.Fail<ArgsOptions>("File appsettings.json not exits. Check or create file.");

    try
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddCommandLine(strings)
            .Build();
        var argsOptions = configuration.Get<ArgsOptions>();
        return argsOptions ?? Result.Fail<ArgsOptions>("Error to get app options. Check app arguments/");
    }
    catch (IOException ex)
    {
        return Result.Fail<ArgsOptions>($"Error to open file. Maybe file blocked by another app\n{ex}");
    }
    catch (Exception ex)
    {
        return Result.Fail<ArgsOptions>(ex.ToString());
    }
}

LoadAppOptions(args)
    .Then<ArgsOptions>(AppOptionsValidator.ValidatePathsInOptions)
    .Then(appOptions =>
    {
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

        return VisualizationOptionsConverter.ConvertOptions(appOptions)
            .Then(visualizations.DrawCloud)
            .Then(bitmap =>
                {
                    AppOptionsValidator.GetImageFormat(appOptions.FileExtension)
                        .Then(imageFormat =>
                            bitmap.Save(Path.Combine(appOptions.DirectoryToSaveFile, string.Concat(appOptions.SaveFileName, ".", appOptions.FileExtension.ToLower())), imageFormat));
                }
            );
    }).OnFail(Console.WriteLine);


Console.Read();