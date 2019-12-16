using System.Collections.Generic;
using TagsCloud.Interfaces;
using System.Linq;
using TagsCloud.ErrorHandling;

namespace TagsCloud.WordCounters
{
    public class DictionaryBasedCounter: IWordCounter
    {
        public Result<IEnumerable<(string word, int frequency)>> GetWordsStatistics(IEnumerable<string> words) => 
            Result.Of(() => words.GroupBy(word => word).Select(group => (group.Key, group.Count())));
    }
}
