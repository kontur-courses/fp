using System.Collections.Generic;
using Result;
using TagCloudVisualization;

namespace TagCloudCreation
{
    public interface ITagCloudStatsGenerator
    {
        Result<IEnumerable<WordInfo>> GenerateStats(IEnumerable<string> words);
    }
}
