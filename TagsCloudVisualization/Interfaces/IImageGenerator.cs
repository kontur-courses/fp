#region

using System.Collections.Generic;
using System.Drawing;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface IImageGenerator
    {
        Result<Bitmap> DrawTagCloudBitmap(IEnumerable<ITag> tags);
        Result<Graphics> GetGraphics();
        Result<Font> GetFont();
    }
}