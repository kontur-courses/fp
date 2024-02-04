using TagsCloudCore.Utils;
using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCore.WordProcessing.WordFiltering;

public class DefaultWordFilter : IWordFilter
{
    private readonly IEnumerable<IWordProvider> _wordsToExcludeSources;

    public DefaultWordFilter(IEnumerable<IWordProvider> wordsToExcludeSources)
    {
        _wordsToExcludeSources = wordsToExcludeSources;
    }

    public Result<string[]> FilterWords(string[] words, WordProviderInfo wordsToExclude)
    {
        if (wordsToExclude.ResourceLocation == "")
            return words;
        var source = _wordsToExcludeSources.SingleOrDefault(p => p.Match(wordsToExclude.Type));
        if (source is null)
            return Result.Fail<string[]>("Couldn't find the provided word provider type implementation.");
        return source.GetWords(wordsToExclude.ResourceLocation)
            .ReplaceError(_ => "Failed to read from the word filter file. Make sure you correctly specified the file path in the configuration.")
            .Then(toExclude => WordProcessingUtils.RemoveDuplicates(toExclude.Select(w => w.Trim()).Select(w => w.ToLower())))
            .Then(toExclude => words.Where(w => !toExclude.Contains(w)).ToArray());
    }
}