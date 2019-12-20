using System.Collections.Generic;
using FunctionalTools;

namespace TagsCloudGenerator.WordsHandler
{
    public interface IWordHandler
    {
        Result<Dictionary<string, int>> GetValidWords(Dictionary<string, int> wordToCount);
    }
}