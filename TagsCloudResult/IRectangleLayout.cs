using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult
{
    public interface IRectangleLayout
    {
        Result<Rectangle> PutNextRectangle(Size size);
        List<Rectangle> GetCoordinatesToDraw();
    }
}
