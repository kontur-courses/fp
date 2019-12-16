using System.Collections.Generic;
using TextConfiguration.WordFilters;
using TextConfiguration.WordProcessors;
using System.Linq;

namespace TextConfiguration
{
    public class TextPreprocessor : ITextPreprocessor
    {
        private readonly IWordFilter[] filters;
        private readonly IWordProcessor wordProcessor;

        public TextPreprocessor(IWordFilter[] filters, IWordProcessor wordProcessor)
        {
            this.filters = filters;
            this.wordProcessor = wordProcessor;
        }

        public Result<List<string>> PreprocessText(string text)
        {
            return Result.Of(() =>
                text
                .Split()
                .Where(wrd => !filters.Any(fltr => fltr.ShouldExclude(wrd).GetValueOrThrow()))
                .Select(wrd => wordProcessor.ProcessWord(wrd).GetValueOrThrow())
                .ToList());
        }
    }
}
