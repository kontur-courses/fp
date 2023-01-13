using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudVisualization;
using TagsCloudVisualization.CLI;
using TagsCloudVisualization.CLI.Extensions;

var result = Parser.Default.ParseArguments<Options>(args);
if (!result.Errors.Any())
{
    var directoryPath = Path.GetFullPath(result.Value.OutputDirectory ?? Options.DefaultOutputDirectory);
    if (!Directory.Exists(directoryPath))
    {
        Directory.CreateDirectory(directoryPath);
    }

    var services = new ServiceCollection();
    var filepath = Path.Combine(directoryPath, CreateFileName());

    result.Value.GetVisualizationSettings()
        .Then(settings => services.AddTagCloudVisualization(settings))
        .Then(s => s.BuildServiceProvider().GetRequiredService<Visualizer>())
        .Then(visualizer => visualizer.Visualize(filepath, result.Value.TagCount))
        .OnSuccess(() => Console.WriteLine("Tags cloud generated"))
        .OnFail(Console.WriteLine);
}

string CreateFileName()
{
    return $"Cloud_{DateTime.Now:dd-MM-yy_hh-mm-ss}";
}