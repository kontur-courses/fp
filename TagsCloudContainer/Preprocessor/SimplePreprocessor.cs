using System.Collections.Generic;
using TagsCloudContainer.Converter;
using TagsCloudContainer.Filter;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.Preprocessor
{
    public class SimplePreprocessor : IPreprocessor
    {
        private readonly IEnumerable<IWordsConverter> converters;
        private readonly IEnumerable<IFilter> filters;

        public SimplePreprocessor(IEnumerable<IWordsConverter> converters, IEnumerable<IFilter> filters)
        {
            this.converters = converters;
            this.filters = filters;
        }

        public Result<IEnumerable<string>> PrepareWords(IEnumerable<string> words)
        {
            foreach (var convert in converters)
                words = convert.Convert(words);

            var newWords = words.AsResult();

            foreach (var filter in filters)
                newWords = newWords.Then(x => filter.FilterOut(x));

            return newWords;
        }
    }
}