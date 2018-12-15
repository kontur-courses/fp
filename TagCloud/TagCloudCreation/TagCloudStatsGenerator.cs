using System;
using System.Collections.Generic;
using System.Linq;
using Functional;
using TagCloudVisualization;

namespace TagCloudCreation
{
    public class TagCloudStatsGenerator : ITagCloudStatsGenerator
    {
        public Result<IEnumerable<WordInfo>> GenerateStats(IEnumerable<string> words)
        {
            return words.GroupBy(w => w, StringComparer.InvariantCulture)
                        .Select(g => new WordInfo(g.Key, g.Count())).AsResult();
        }
    }
}
