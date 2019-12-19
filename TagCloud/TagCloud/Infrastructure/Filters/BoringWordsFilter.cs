using ResultOF;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud
{
    public class BoringWordsFilter : IFilter
    {
        private readonly BoringWord[] boringWords;

        public bool IsChecked { get; set; }

        public BoringWordsFilter(BoringWord[] boringWords)
        {
            this.boringWords = boringWords;
            IsChecked = true;
        }

        public Result<string[]> FilterWords(string[] words)
        {
            if (words == null)
                return Result.Fail<string[]>("Words cannot be null");
            var boringWords = GetBoringWords();
            var boringWordsHashet = new HashSet<string>(boringWords);
            return GetFilteredWords(words, boringWordsHashet);
        }

        private IEnumerable<string> GetBoringWords()
        {
            return boringWords
                .Where(word => word.IsChecked)
                .Select(word => word.Value);
        }

        private Result<string[]> GetFilteredWords(string[] words, HashSet<string> boringWords)
        {
            return words
                .Where(word => !boringWords.Contains(word))
                .ToArray();
        }
    }
}
