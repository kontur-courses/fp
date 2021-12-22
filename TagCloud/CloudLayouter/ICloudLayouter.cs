using System.Drawing;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    RectangleF PutNextRectangle(SizeF rectangleSize);
}