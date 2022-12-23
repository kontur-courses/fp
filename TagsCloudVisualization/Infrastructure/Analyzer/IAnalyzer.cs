using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.Analyzer
{
    public interface IAnalyzer
    {
        public Result<IEnumerable<IWeightedWord>> CreateWordFrequenciesSequence(IEnumerable<string> words);
    }
}