using System.Collections.Generic;
using System.Linq;
using TextConfiguration.TextReaders;

namespace TextConfiguration.WordFilters
{
    public class CustomBoringWordsFilter : IWordFilter
    {
        private readonly Result<List<string>> excludedWords;

        public CustomBoringWordsFilter(ITextReader reader, string filename)
        {
            excludedWords = reader.ReadText(filename)
                .Then(str => str.Split())
                .Then(ar => ar.ToList());
        }

        public Result<bool> ShouldExclude(string word)
        {
            return excludedWords.Then(l => l.Contains(word.ToLower()));
        }
    }
}
