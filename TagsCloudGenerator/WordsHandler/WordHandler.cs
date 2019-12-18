using System.Collections.Generic;
using System.Linq;
using TagsCloudGenerator.WordsHandler.Converters;
using TagsCloudGenerator.WordsHandler.Filters;

namespace TagsCloudGenerator.WordsHandler
{
    public class WordHandler : IWordHandler
    {
        private readonly List<IWordsFilter> filters;
        private readonly List<IConverter> converters;

        public WordHandler(IEnumerable<IWordsFilter> filters, IEnumerable<IConverter> converters)
        {
            this.filters = filters.ToList();
            this.converters = converters.ToList();
        }

        public Result<Dictionary<string, int>> GetValidWords(Dictionary<string, int> wordToCount)
        {
            return wordToCount
                .AsResult()
                .Then(words => ApplyConverters(words))
                .Then(words => ApplyFilters(words))
                .RefineError("Couldn't get valid words");
        }

        private Result<Dictionary<string, int>> ApplyFilters(Result<Dictionary<string, int>> words)
        {
            return filters
                .Aggregate(words, (current, filter) => current.Then(filter.Filter))
                .RefineError("Applying filters got error");
        }

        private Result<Dictionary<string, int>> ApplyConverters(Result<Dictionary<string, int>> words)
        {
            return converters
                .Aggregate(words, (current, converter) => current.Then(converter.Convert))
                .RefineError("Applying converters got error");
        }
    }
}