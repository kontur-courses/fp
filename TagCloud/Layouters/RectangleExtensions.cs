using System.Drawing;

namespace TagCloud.Layouters
{
    internal static class RectangleExtensions
    {
        internal static Point GetCenter(this Rectangle rectangle)
        {
            var x = rectangle.X + rectangle.Width / 2;
            var y = rectangle.Y + rectangle.Height / 2;
            return new Point(x, y);
        }

        internal static bool InsideSize(this Rectangle rectangle, Size size)
        {
            return rectangle.Top >= 0
                   && rectangle.Bottom <= size.Height
                   && rectangle.Left >= 0
                   && rectangle.Right <= size.Width;
        }
    }
}
