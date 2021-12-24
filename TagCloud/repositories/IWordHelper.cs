using System.Collections.Generic;

namespace TagCloud.repositories
{
    public interface IWordHelper
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
        Result<IEnumerable<string>> ConvertWords(IEnumerable<string> words);
        Result<IEnumerable<WordStatistic>> GetWordStatistics(IEnumerable<string> words);
    }
}