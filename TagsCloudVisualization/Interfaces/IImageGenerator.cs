using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface IImageGenerator
    {
        Result<Bitmap> DrawTagCloudBitmap(IEnumerable<ITag> tags);
    }
}