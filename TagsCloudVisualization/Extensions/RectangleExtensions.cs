using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle MoveToCenter(this Rectangle rectangle, Point center)
        {
            rectangle.Location = new Point(center.X - rectangle.Width / 2, center.Y - rectangle.Height / 2);
            
            return rectangle;
        }

        public static bool FitsToImage(this Rectangle rectangle, Size imageSize)
        {
            return rectangle.Location.X + rectangle.Width <= imageSize.Width &&
                   rectangle.Location.Y + rectangle.Height <= imageSize.Height;
        }
    }
}