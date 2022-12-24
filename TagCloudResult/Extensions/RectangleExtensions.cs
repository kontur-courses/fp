using System.Drawing;

namespace TagCloudResult.Extensions;

public static class RectangleExtensions
{
    public static Point GetCenter(this Rectangle rectangle)
    {
        var topLeftPoint = rectangle.Location;
        return new Point(topLeftPoint.X + rectangle.Width / 2, topLeftPoint.Y + rectangle.Height / 2);
    }

    public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> otherRectangles)
    {
        return otherRectangles.Any(rect => rect.IntersectsWith(rectangle));
    }
}