using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCore.WordProcessing.WordFiltering;

public interface IWordFilter
{
    public Result<string[]> FilterWords(string[] words, WordProviderInfo wordsToExclude);
}