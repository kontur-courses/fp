using System.Collections.Generic;
using TagCloud.TagCloudVisualization.Analyzer;

namespace TagCloud.TagCloudVisualization.Layouter
{
    public interface ITagCloudLayouter
    {
        List<Tag> GetCloudTags(Dictionary<string, int> weightedWords);
    }
}