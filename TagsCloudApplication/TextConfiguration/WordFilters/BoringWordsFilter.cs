using System.Collections.Generic;

namespace TextConfiguration.WordFilters
{
    public class BoringWordsFilter : IWordFilter
    {
        private readonly List<string> excludedWords =
            new List<string>() { "boring", "bored", "boooored", "зевнул" };

        public Result<bool> ShouldExclude(string word)
        {
            return excludedWords.Contains(word.ToLower());
        }
    }
}
