using Microsoft.Extensions.DependencyInjection;
using TagsCloud.Contracts;
using TagsCloud.Extensions;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud;

public class TagCloudEngine
{
    private readonly IServiceProvider serviceProvider;

    public TagCloudEngine(
        IInputProcessorOptions inputOptions,
        ICloudProcessorOptions cloudOptions,
        IOutputProcessorOptions outputOptions)
    {
        serviceProvider = new ServiceCollection()
                          .AddAllInjections()
                          .AddSingleton(inputOptions)
                          .AddSingleton(cloudOptions)
                          .AddSingleton(outputOptions)
                          .BuildServiceProvider();
    }

    public Result<HashSet<WordTagGroup>> GenerateTagCloud(string inputFile, string outputFile)
    {
        var textProcessor = serviceProvider.GetRequiredService<IInputProcessor>();
        var cloudProcessor = serviceProvider.GetRequiredService<ICloudProcessor>();
        var outputProcessor = serviceProvider.GetRequiredService<IOutputProcessor>();

        var groupsResult = textProcessor
                           .CollectWordGroupsFromFile(inputFile)
                           .Then(cloudProcessor.SetFonts)
                           .Then(cloudProcessor.SetPositions)
                           .Then(cloudProcessor.SetColors)
                           .Then(groups =>
                               outputProcessor.SaveVisualization(groups, outputFile));

        return groupsResult;
    }
}