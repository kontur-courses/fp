using Functional;
using System.Collections.Generic;
using TagCloudVisualization;

namespace TagCloudCreation
{
    public interface ITagCloudStatsGenerator
    {
        Result<IEnumerable<WordInfo>> GenerateStats(IEnumerable<string> words);
    }
}
