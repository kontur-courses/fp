using System.Collections.Generic;
using ResultOf;

namespace TagCloud
{
    public interface IWordsHandler
    {
        Result<Dictionary<string, int>> GetWordsAndCount(string path);
        Result<Dictionary<string, int>> Conversion(Dictionary<string, int> wordsAndCount);
    }
}