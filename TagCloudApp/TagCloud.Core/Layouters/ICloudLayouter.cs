using System.Drawing;
using TagCloud.Core.Util;

namespace TagCloud.Core.Layouters
{
    public interface ICloudLayouter
    {
        Result<RectangleF> PutNextRectangle(SizeF rectangleSize);
        Result<None> RefreshWith(PointF newCenterPoint);
    }
}