using System.Drawing;

namespace TagCloud.Common.Extensions;

public static class RectangleExtension
{
    public static Point FindCenter(this Rectangle rectangle)
    {
        return new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
    }
}