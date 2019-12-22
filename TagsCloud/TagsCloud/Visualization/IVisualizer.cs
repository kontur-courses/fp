using System.Collections.Generic;
using System.Drawing;
using TagsCloud.ErrorHandler;

namespace TagsCloud.Visualization
{
    public interface IVisualizer
    {
        Bitmap GetCloudVisualization(IEnumerable<Tag.Tag> tags);
    }
}