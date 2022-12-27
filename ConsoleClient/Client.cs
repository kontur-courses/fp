using FluentResults;
using TagCloud.Abstractions;

namespace ConsoleClient;

public class Client
{
    private readonly ICloudCreator creator;
    private readonly ICloudDrawer drawer;
    private readonly IWordsLoader loader;
    private readonly IEnumerable<IWordsProcessor> processors;
    private readonly IWordsTagger tagger;

    public Client(
        IWordsLoader loader,
        IEnumerable<IWordsProcessor> processors,
        IWordsTagger tagger,
        ICloudCreator creator,
        ICloudDrawer drawer)
    {
        this.loader = loader;
        this.processors = processors;
        this.tagger = tagger;
        this.creator = creator;
        this.drawer = drawer;
    }

    public Result Execute(string resultFilepath)
    {
        var loadResult = loader.Load();
        if (loadResult.IsFailed) return Result.Fail(loadResult.Errors);
        
        var words = loadResult.Value;
        foreach (var processor in processors)
        {
            var processResult = processor.Process(words);
            if (processResult.IsFailed) return Result.Fail(processResult.Errors);
            words = processResult.Value;
        }
        
        var toTagsResult = tagger.ToTags(words);
        if (toTagsResult.IsFailed) return Result.Fail(toTagsResult.Errors);
        var tags = toTagsResult.Value;
        
        var createCloudResult = creator.CreateTagCloud(tags);
        if (createCloudResult.IsFailed) return Result.Fail("Occured internal error.");
        var drawableTags = createCloudResult.Value;
        
        var bitmap = drawer.Draw(drawableTags);
        bitmap.Save(resultFilepath);
        return Result.Ok().WithSuccess($"All Ok. Tag Cloud saved to '{Path.GetFullPath(resultFilepath)}'.");
    }
}