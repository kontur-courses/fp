using ResultOF;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud
{
    public class BoringWordsFilter : IFilter
    {
        private readonly BoringWord[] boringWords;

        public bool IsChecked { get; set; }

        public string Name { get; }

        public BoringWordsFilter(BoringWord[] boringWords)
        {
            this.boringWords = boringWords;
            IsChecked = true;
            Name = "Boring words filter";
        }

        public Result<string[]> FilterWords(string[] wordsResult)
        {
            if (wordsResult == null)
                return Result.Fail<string[]>("Words cannot be null");
            var words = wordsResult;
            var boringWords = GetBoringWords();
            var boringWordsHashet = new HashSet<string>(boringWords);
            return GetFilteredWords(words, boringWordsHashet);
        }

        private IEnumerable<string> GetBoringWords()
        {
            return boringWords
                .Where(word => word.IsChecked)
                .Select(word => word.Name);
        }

        private Result<string[]> GetFilteredWords(string[] words, HashSet<string> boringWords)
        {
            return words
                .Where(word => !boringWords.Contains(word))
                .ToArray();
        }
    }
}
