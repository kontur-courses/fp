using System.Collections.Generic;
using System.Linq;
using TagCloud.Provider;

namespace TagCloud.Analyzers
{
    public class BoringWordsFilter : IWordFilter
    {
        private readonly IWordProvider wordProvider;
        //private readonly HashSet<string> boringWords = new();

        public BoringWordsFilter(IWordProvider wordProvider)
        {
            this.wordProvider = wordProvider;
        }

        public IEnumerable<string> Analyze(IEnumerable<string> words)
        {
            var boringWords = wordProvider.Words.ToHashSet();
            return words.Where(w => !boringWords.Contains(w));
        }
    }
}
