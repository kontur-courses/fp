using System.Collections.Generic;
using System.Drawing;
using TagsCloud.ErrorHandling;

namespace TagsCloud.TagsCloudVisualization
{
    public interface ITagsCloudVisualizer
    {
        Result<Bitmap> GetCloudVisualization(List<Tag> tags);
    }
}
