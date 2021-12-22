using System.Collections.Generic;

namespace TagCloud.repositories
{
    public interface IWordHelper
    {
        Result<List<string>> FilterWords(IEnumerable<string> words);
        Result<List<string>> ConvertWords(IEnumerable<string> words);
        IEnumerable<WordStatistic> GetWordStatistics(IEnumerable<string> words);
    }
}