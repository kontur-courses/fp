using CSharpFunctionalExtensions;
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

        public string[] FilterWords(string[] words)
        {
            var boring = boringWords
                .Where(word => word.IsChecked)
                .Select(word => word.Name);
            var boringWordsHashSet = new HashSet<string>(boring);
            var filteredWords = words
                .Where(word => !boringWordsHashSet.Contains(word))
                .ToArray();
            return filteredWords;
        }
    }
}
