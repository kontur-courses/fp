using System.Drawing;

namespace TagCloud.Extensions
{
    public static class SizeExtension
    {
        public static bool IsNotCorrectSize(this Size rectangleSize)
        {
            return rectangleSize.Width <= 0 || rectangleSize.Height <= 0;
        }
    }
}