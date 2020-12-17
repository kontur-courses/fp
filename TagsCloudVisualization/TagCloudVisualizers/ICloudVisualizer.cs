using System.Collections.Generic;
using TagsCloudVisualization.Tags;

namespace TagsCloudVisualization.TagCloudVisualizers
{
    public interface ICloudVisualizer
    {
        Result<None> PrintTagCloud(IEnumerable<Tag> tags);
    }
}