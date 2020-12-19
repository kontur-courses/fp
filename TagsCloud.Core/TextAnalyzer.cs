using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class TextAnalyzer
    {
        private readonly HunspellFactory hunspellFactory;
        private readonly PathSettings pathSettings;

        public TextAnalyzer(HunspellFactory hunspellFactory, PathSettings pathSettings)
        {
            this.hunspellFactory = hunspellFactory;
            this.pathSettings = pathSettings;
        }

        public Result<List<(string, int)>> GetWordByFrequency(List<string> text,
            Func<Dictionary<string, int>, IOrderedEnumerable<KeyValuePair<string, int>>> sort)
        {
            return hunspellFactory.CreateHunspell(pathSettings)
                .Then(hun => text.Select(x => x.Normalize(hun)))
                .Then(collection => collection.GroupBy(x => x))
                .Then(groups => groups.ToDictionary(x => x.Key, x => x.Count()))
                .Then(sort)
                .Then(collection => collection.Select(x => (x.Key, x.Value)))
                .Then(collection => collection.ToList());
        }
    }
}