using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface ICloudLayouter
    {
        Point Center { get; }
        List<Rectangle> Rectangles { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}