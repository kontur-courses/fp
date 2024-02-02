using TagsCloudCore.Utils;
using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCore.WordProcessing.WordFiltering;

public class DefaultWordFilter : IWordFilter
{
    private readonly IWordProvider _wordsToExcludeSource;
    private readonly string _resourceLocation;

    public DefaultWordFilter(IWordProvider wordsToExcludeSource, string resourceLocation)
    {
        _wordsToExcludeSource = wordsToExcludeSource;
        _resourceLocation = resourceLocation;
    }

    public Result<string[]> FilterWords(string[] words)
    {
        return _wordsToExcludeSource.GetWords(_resourceLocation)
            .Then(toExclude => WordProcessingUtils.RemoveDuplicates(toExclude.Select(w => w.ToLower())))
            .Then(toExclude => words.Where(w => !toExclude.Contains(w)).ToArray());
    }
}