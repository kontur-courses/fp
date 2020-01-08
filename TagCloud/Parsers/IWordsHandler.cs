using System.Collections.Generic;
using ResultOf;

namespace TagCloud
{
    public interface IWordsHandler
    {
        Result<Dictionary<string, int>> GetWordsAndCount(IEnumerable<string> words);

        Result<Dictionary<string, int>> RemoveBoringWords(Dictionary<string, int> wordsAndCount,
            string pathToBoringWords);
    }
}