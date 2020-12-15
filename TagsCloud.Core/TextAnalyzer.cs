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

            text.Select(x => x.ToLower().Normalize(hunspell))
                .Where(x => !boringWords.Contains(x))
                .Select(x => wordsFrequency.ContainsKey(x)
                    ? wordsFrequency[x]++
                    : wordsFrequency[x] = 1)
                .ToList();

            return sort(wordsFrequency)
                .Select(x => (x.Key, x.Value))
                .ToList();
        }
    }
}