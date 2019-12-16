using System.Collections.Generic;
using System.Drawing;
using TextConfiguration;

namespace TagsCloudVisualization
{
    public interface ITagCloudVisualizator
    {
        Result<Bitmap> VisualizeCloudTags(IReadOnlyCollection<CloudTag> cloudTags);
    }
}
