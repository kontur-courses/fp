using System;
using System.Collections.Generic;
using System.Linq;
using NHunspell;

namespace TagsCloud.Core
{
    public static class TextAnalyzer
    {
        public static List<(string, int)> GetWordByFrequency(string[] text, HashSet<string> boringWords,
            Hunspell hunspell, Func<Dictionary<string, int>, IOrderedEnumerable<KeyValuePair<string, int>>> sort)
        {
            return sort(text
                .Select(x => x.ToLower().Normalize(hunspell))
                .Where(x => !boringWords.Contains(x))
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count()))
                .Select(x => (x.Key, x.Value))
                .ToList();
        }
    }
}