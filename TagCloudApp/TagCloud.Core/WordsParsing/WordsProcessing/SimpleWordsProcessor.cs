using System.Collections.Generic;
using System.Linq;
using TagCloud.Core.Util;
using TagCloud.Core.WordsParsing.WordsProcessing.WordsProcessingUtilities;

namespace TagCloud.Core.WordsParsing.WordsProcessing
{
    public class SimpleWordsProcessor : IWordsProcessor
    {
        private readonly IWordsProcessingUtility[] utilities;

        public SimpleWordsProcessor(IWordsProcessingUtility[] utilities)
        {
            this.utilities = utilities;
        }

        public Result<IEnumerable<TagStat>> Process(IEnumerable<string> words, HashSet<string> boringWords = null,
            int? maxUniqueWordsCount = null)
        {
            if (words is null)
                return Result.Fail<IEnumerable<TagStat>>("Given words can't be null");

            var wordsCounter = utilities.Aggregate(words,
                    (currentWords, processingUtility) => processingUtility.Process(currentWords))
                .Where(word => boringWords == null || !boringWords.Contains(word))
                .GroupBy(word => word)
                .ToDictionary(g => g.Key, g => g.Count());

            return HandleWordsCounter(wordsCounter, maxUniqueWordsCount);
        }

        private static Result<IEnumerable<TagStat>> HandleWordsCounter(Dictionary<string, int> wordsCounter,
            int? maxUniqueWordsCount)
        {
            var allTagsStats = new List<TagStat>();
            foreach (var word in wordsCounter.Keys)
            {
                var count = wordsCounter[word];
                allTagsStats.Add(new TagStat(word, count));
            }

            if (maxUniqueWordsCount.HasValue)
            {
                return maxUniqueWordsCount.Value >= 0
                    ? allTagsStats.OrderBy(ts => ts.RepeatsCount).Take(maxUniqueWordsCount.Value).ToList()
                    : Result.Fail<IEnumerable<TagStat>>("MaxUniqueWordsCount should be positive number");
            }

            return allTagsStats;
        }
    }
}