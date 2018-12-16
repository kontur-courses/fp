using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.TagsCloudVisualization.ColorSchemes.ColorSchemes.SizeDefiners
{
    public interface ITagsCloudVisualizer
    {
        Bitmap GetCloudVisualization(List<Tag> tags);
    }
}
