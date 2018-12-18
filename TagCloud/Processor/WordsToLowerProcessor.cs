using System.Collections.Generic;
using System.Linq;
using TagCloud.Data;

namespace TagCloud.Processor
{
    public class WordsToLowerProcessor : IWordsProcessor
    {
        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return Result.Ok(words.Select(word => word.ToLower()));
        }
    }
}