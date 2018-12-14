using System.Collections.Generic;
using System.Linq;

namespace TagCloud.Words
{
    public class WordManager
    {
        private readonly IWordFilter[] filters;
        private readonly IWordProcessor[] processors;

        public WordManager(IWordFilter[] filters, IWordProcessor[] processors)
        {
            this.filters = filters;
            this.processors = processors;
        }

        public Result<IEnumerable<string>> ApplyFilters(IEnumerable<string> words)
        {
            var filteredWords = filters.Aggregate(words,
                (currentWords, currentFilter) => currentFilter.Filter(currentWords).Value);
            return processors.Aggregate(filteredWords,
                (currentWords, currentFilter) => currentFilter.Preprocess(currentWords)).AsResult();
        }
    }
}