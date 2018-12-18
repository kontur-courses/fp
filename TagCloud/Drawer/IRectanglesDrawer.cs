using System.Collections.Generic;
using System.Drawing;
using TagCloud.Data;

namespace TagCloud.Drawer
{
    public interface IRectanglesDrawer
    {
        Result<Bitmap> CreateImage(IEnumerable<Rectangle> rectangles);
    }
}