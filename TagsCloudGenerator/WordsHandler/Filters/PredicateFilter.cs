using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalTools;

namespace TagsCloudGenerator.WordsHandler.Filters
{
    public class PredicateFilter : IWordsFilter
    {
        private readonly Predicate<KeyValuePair<string, int>> removePredicate;

        public PredicateFilter(Predicate<KeyValuePair<string, int>> removePredicate)
        {
            this.removePredicate = removePredicate;
        }

        public Result<Dictionary<string, int>> Filter(Dictionary<string, int> wordToCount)
        {
            return Result.Of(() => wordToCount
                .Where(el => !removePredicate(el))
                .ToDictionary(el => el.Key, el => el.Value));
        }
    }
}