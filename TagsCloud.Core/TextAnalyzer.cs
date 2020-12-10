using System;
using System.Collections.Generic;
using System.Linq;
using NHunspell;

namespace TagsCloud.Core
{
    public static class TextAnalyzer
    {
        public static List<(string, int)> GetWordByFrequency(string[] text, HashSet<string> boringWords,
            Hunspell hunspell, Func<Dictionary<string, int>, IEnumerable<KeyValuePair<string, int>>> sort)
        {
            var wordsFrequency = new Dictionary<string, int>();
            foreach (var word in text)
            {
                var correctWord = NormalizeWord(word.ToLower(), hunspell);
                if (boringWords.Contains(correctWord))
                    continue;
                if (!wordsFrequency.ContainsKey(correctWord))
                    wordsFrequency[correctWord] = 1;
                else wordsFrequency[correctWord]++;
            }

            return sort(wordsFrequency)
                .Select(x => (x.Key, x.Value))
                .ToList();
        }

        private static string NormalizeWord(string word, Hunspell hunspell)
        {
            var stems = hunspell.Stem(word);
            return stems.Count > 0 ? stems.FirstOrDefault() : word;
        }
    }
}