using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudVisualization.Preprocessing
{
    public class Preprocessor
    {
        private readonly IFilter[] filters;
        private readonly IWordTransformer[] transformers;

        public Preprocessor(IFilter[] filters, IWordTransformer[] transformers)
        {
            this.filters = filters;
            this.transformers = transformers;
        }

        public Result<IEnumerable<string>> Preprocess(IEnumerable<string> words)
        {
            return filters.Aggregate(words.AsResult(), 
                        (current, filter) => current.Then(filter.FilterWords))
                    .Then(filteredWords => filteredWords.Select(ApplyAllTransforms));
        }

        private string ApplyAllTransforms(string word)
        {
            var transformedWord = word;
            foreach (var transformer in transformers)
                transformedWord = transformer.TransformWord(transformedWord);
            return transformedWord;
        }
    }
}
