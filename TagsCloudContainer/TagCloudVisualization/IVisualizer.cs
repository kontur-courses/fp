using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.TagCloudVisualization
{
    public interface IVisualizer
    {
        Result<Bitmap> Visualize(IEnumerable<TagCloudItem> items);
    }
}