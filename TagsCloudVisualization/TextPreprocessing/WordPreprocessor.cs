using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.TextFilters;
using TagsCloudVisualization.WordConverters;

namespace TagsCloudVisualization.TextPreprocessing
{
    public class WordPreprocessor
    {
        private readonly IEnumerable<ITextFilter> filters;
        private readonly IEnumerable<IWordConverter> wordConverters;

        public WordPreprocessor(IEnumerable<ITextFilter> filters, IEnumerable<IWordConverter> wordConverters)
        {
            this.filters = filters;
            this.wordConverters = wordConverters;
        }

        public Result<IEnumerable<string>> GetPreprocessedWords(IEnumerable<string> words)
        {
            return Result.Of(() =>
                    wordConverters.Aggregate(words, (current, wordConverter) => wordConverter.ConvertWords(current).Value))
                .RefineError("Failed to convert words")
                .Then(convertedWords => filters.Aggregate(convertedWords,
                    (current, textFilter) => textFilter.FilterWords(current).Value))
                .RefineError("Failed to filter words");
        }
    }
}