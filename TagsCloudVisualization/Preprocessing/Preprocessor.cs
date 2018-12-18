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
            Result<IEnumerable<string>> filterResult = words.AsResult();
            foreach (var filter in filters)
                filterResult = filterResult.Then(filteredWords => filter.FilterWords(filteredWords));
            
            return filterResult.Then(filteredWords => filteredWords.Select(ApplyAllTransforms));
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
