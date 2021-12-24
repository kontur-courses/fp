using System.Drawing;

namespace TagCloud.CloudLayouter;

public interface ICloudLayouter
{
    Result<RectangleF> PutNextRectangle(SizeF rectangleSize);
}