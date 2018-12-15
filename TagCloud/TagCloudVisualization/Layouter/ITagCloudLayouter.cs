using System.Collections.Generic;
using TagCloud.TagCloudVisualization.Analyzer;

namespace TagCloud.TagCloudVisualization.Layouter
{
    public interface ITagCloudLayouter
    {
        Result<IEnumerable<Tag>> GetCloudTags(Dictionary<string, int> weightedWords);
    }
}