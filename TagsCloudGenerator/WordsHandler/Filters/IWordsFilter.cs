using System.Collections.Generic;
using FunctionalTools;

namespace TagsCloudGenerator.WordsHandler.Filters
{
    public interface IWordsFilter
    {
        Result<Dictionary<string, int>> Filter(Dictionary<string, int> wordToCount);
    }
}