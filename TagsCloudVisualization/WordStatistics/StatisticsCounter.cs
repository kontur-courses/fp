using System.Collections.Generic;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.WordStatistics
{
    public class StatisticsCounter
    {
        private readonly IStatisticsCollector[] statCollectors;

        public StatisticsCounter(IStatisticsCollector[] collectors)
        {
            statCollectors = collectors;
        }

        public Result<AnalyzedText> GetAnalyzedText(Word[] words)
        {
            return Result.Of(() => AnalyzeText(words));
        }

        private AnalyzedText AnalyzeText(Word[] words)
        {
            var statistics = new Dictionary<WordStatistics, int>();
            foreach (var collector in statCollectors)
                foreach (var (wordStatistics, value) in collector.GetStatistics(words))
                    statistics[wordStatistics] = value;
            return new AnalyzedText(words, statistics);
        }
    }
}