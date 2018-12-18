using System.Collections.Generic;
using TagsCloudContainer.ResultOf;
using TagsCloudContainer.Tag;

namespace TagsCloudContainer.Visualizer
{
    public interface IVisualizer
    {
        Result<byte[]> Visualize(IEnumerable<ITag> tags);
    }
}