using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloud.Visualization.Extensions
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> other) =>
            other.Any(rectangle.IntersectsWith);

        public static Point GetCenter(this Rectangle rectangle) =>
            new(rectangle.Left + rectangle.Width / 2,
                rectangle.Top + rectangle.Height / 2);
    }

    public static class RectangleIEnumerableExtensions
    {
        public static Size GetSize(this IEnumerable<Rectangle> rectangles)
        {
            rectangles = rectangles.ToArray();
            var maxRight = rectangles.Max(x => x.Right);
            var minLeft = rectangles.Min(x => x.Left);
            var maxBottom = rectangles.Max(x => x.Bottom);
            var minTop = rectangles.Min(x => x.Top);

            return new Size(Math.Abs(maxRight - minLeft), Math.Abs(maxBottom - minTop));
        }
    }
}