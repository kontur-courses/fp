using ResultOf;
using System.Collections.Generic;
using TagsCloudContainer.WordProcessing.Converting;
using TagsCloudContainer.WordProcessing.Filtering;

namespace TagsCloudContainer.WordProcessing
{
    public class WordProcessor : IWordProcessor
    {
        private readonly IWordConverter wordConverter;
        private readonly IWordFilter wordFilter;

        public WordProcessor(IWordConverter wordConverter, IWordFilter wordFilter)
        {
            this.wordConverter = wordConverter;
            this.wordFilter = wordFilter;
        }

        public Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words)
        {
            return wordConverter.ConvertWords(words)
                .RefineError("Failed to convert words")
                .Then(convertedWords => wordFilter.FilterWords(convertedWords))
                .RefineError("Failed to filter words");
        }
    }
}