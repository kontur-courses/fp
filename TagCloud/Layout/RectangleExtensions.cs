using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public static class RectangleExtensions
    {
        public static Rectangle Move(this Rectangle rectangle, int x, int y)
        {
            rectangle.Location = new Point(rectangle.X + x, rectangle.Y + y);
            return rectangle;
        }

        public static bool HasCollisionsWith(this Rectangle rectangle, Rectangle[] otherRectangles)
        {
            return otherRectangles.Any(r => r.IntersectsWith(rectangle));
        }
    }
}
