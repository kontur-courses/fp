using System.Collections.Generic;

namespace TagsCloudGenerator.WordsHandler
{
    public interface IWordHandler
    {
        Result<Dictionary<string, int>> GetValidWords(Dictionary<string, int> wordToCount);
    }
}