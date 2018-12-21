using System.Collections.Generic;
using TagCloud.IntermediateClasses;

namespace TagCloud.Interfaces
{
    public interface IStatisticsCollector
    {
        Result<IEnumerable<FrequentedWord>> GetStatistics(IEnumerable<string> words);
    }
}