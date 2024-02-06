using Microsoft.Extensions.DependencyInjection;
using TagsCloud.CustomAttributes;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloud.Results;
using TagsCloud.Validators;
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

    public Result<HashSet<WordTagGroup>> SaveVisualization(HashSet<WordTagGroup> wordGroups, string filePath)
    {
        return PathValidator
               .ValidateDirectory(Path.GetDirectoryName(filePath))
               .Then(_ =>
               {
                   new VisualizationBuilder(outputOptions.ImageSize, outputOptions.BackgroundColor)
                       .CreateImageFrom(wordGroups)
                       .SaveAs(filePath, outputOptions.ImageEncoder);
               })
               .Then(_ => wordGroups)
               .RefineError("Can't save image file, because:");
    }
}