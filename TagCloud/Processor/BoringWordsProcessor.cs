using System.Collections.Generic;
using System.Linq;
using TagCloud.Data;

namespace TagCloud.Processor
{
    public class BoringWordsProcessor : IWordsProcessor
    {
        private readonly HashSet<string> boringWords;

        public BoringWordsProcessor(IEnumerable<string> boringWords)
        {
            this.boringWords = new HashSet<string>(boringWords);
        }

        public Result<IEnumerable<string>> Process(IEnumerable<string> words)
        {
            return Result.Ok(words.Where(word => !boringWords.Contains(word)));
        }
    }
}