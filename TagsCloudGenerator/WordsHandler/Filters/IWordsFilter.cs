using System.Collections.Generic;

namespace TagsCloudGenerator.WordsHandler.Filters
{
    public interface IWordsFilter
    {
        Result<Dictionary<string, int>> Filter(Dictionary<string, int> wordToCount);
    }
}