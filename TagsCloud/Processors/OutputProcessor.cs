using Microsoft.Extensions.DependencyInjection;
using TagsCloud.Contracts;
using TagsCloud.CustomAttributes;
using TagsCloud.Extensions;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Processors;

[Injection(ServiceLifetime.Singleton)]
public class OutputProcessor : IOutputProcessor
{
    private readonly IOutputProcessorOptions outputOptions;

    public OutputProcessor(IOutputProcessorOptions outputOptions)
    {
        this.outputOptions = outputOptions;
    }

    public Result<HashSet<WordTagGroup>> SaveVisualization(HashSet<WordTagGroup> wordGroups, string filename)
    {
        try
        {
            new VisualizationBuilder(outputOptions.ImageSize, outputOptions.BackgroundColor)
                .CreateImageFrom(wordGroups)
                .SaveAs(filename, outputOptions.ImageEncoder);

            return wordGroups;
        }
        catch
        {
            return ResultExtensions
                .Fail<HashSet<WordTagGroup>>("Can't save image! Please, check app permissions.");
        }
    }
}