using System.Drawing;

namespace TagsCloudContainer.Extensions
{
    public static class RectangleExtensions
    {
        public static void ValidateSize(this Size rectangleSize, int minValue)
        {
            if (rectangleSize.Width < minValue || rectangleSize.Height < minValue)
            {
                throw new ArgumentException("Width and height of the rectangle must be greater than zero");
            }
        }
    }
}
