using System.Drawing;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Layouters;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}