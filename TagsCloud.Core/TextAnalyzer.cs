using System;
using System.Collections.Generic;
using System.Linq;
using NHunspell;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class TextAnalyzer
    {
        private readonly Result<Hunspell> hunspell;

        public TextAnalyzer(HunspellFactory hunspellFactory)
        {
            hunspell = hunspellFactory.CreateHunspell();
        }

        public Result<List<(string, int)>> GetWordByFrequency(string[] text, HashSet<string> boringWords,
            Func<Dictionary<string, int>, IOrderedEnumerable<KeyValuePair<string, int>>> sort)
        {
            return hunspell
                .Then(hun => text.Select(x => x.ToLower().Normalize(hun)))
                .Then(collection => collection.Where(x => !boringWords.Contains(x)))
                .Then(collection => collection.GroupBy(x => x))
                .Then(groups => groups.ToDictionary(x => x.Key, x => x.Count()))
                .Then(sort)
                .Then(collection => collection.Select(x => (x.Key, x.Value)))
                .Then(collection => collection.ToList());
        }
    }
}