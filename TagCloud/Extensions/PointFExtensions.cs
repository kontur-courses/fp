using System.Drawing;

namespace TagCloud.Extensions;

internal static class PointFExtensions
{
    public static RectangleF GetRectangle(this PointF center, SizeF size)
    {
        return new RectangleF(
            new PointF(center.X - size.Width / 2f, center.Y - size.Height / 2f),
            size);
    }
}