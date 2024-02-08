using System.Drawing;

namespace TagsCloud.CloudLayouter;

public interface ICloudLayouter
{
    List<Rectangle> Rectangles { get; }
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}