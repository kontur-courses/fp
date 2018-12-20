using System.Collections.Generic;
using System.Drawing;
using ResultOfTask;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        List<Rectangle> Rectangles { get; }
        ISpiral Spiral { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}