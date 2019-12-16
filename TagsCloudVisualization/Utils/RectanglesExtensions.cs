using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Utils
{
    public static class RectanglesExtensions
    {
        public static IEnumerable<Point> GetCornerPoints(this Rectangle rectangle)
        {
            yield return new Point(rectangle.Left, rectangle.Bottom);
            yield return new Point(rectangle.Right, rectangle.Bottom);
            yield return new Point(rectangle.Left, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Top);
        }
    }
}