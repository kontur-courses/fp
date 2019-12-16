using System.Collections.Generic;

using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface IWordCounter
    {
        Result<IEnumerable<(string word, int frequency)>> GetWordsStatistics(IEnumerable<string> words);
    }
}
