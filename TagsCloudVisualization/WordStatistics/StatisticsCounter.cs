using System;
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
            var statistics = new Dictionary<WordStatistics, int>();
            foreach (var collector in statCollectors)
            {
                var wordsAnalysisResult = words.AsResult().Then(x => collector.GetStatistics(x));
                if (!wordsAnalysisResult.IsSuccess)
                    return ResultExt.Fail<AnalyzedText>(wordsAnalysisResult.Error);
                foreach (var (wordStatistics, value) in wordsAnalysisResult.Value)
                    statistics[wordStatistics] = value;
            }
            return new AnalyzedText(words, statistics).AsResult();
        }
    }
}