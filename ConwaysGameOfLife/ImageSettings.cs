using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloud
{
    public class ImageSettings
    {
        public readonly Size ImageSize;
        public readonly FontFamily FontFamily;
        public readonly Color Color;
        public readonly ImageFormat ImageFormat;
        public readonly string ImageName;

        public ImageSettings(Size imageSize, FontFamily fontFamily, Color color, ImageFormat imageFormat, string imageName)
        {
            ImageSize = imageSize;
            FontFamily = fontFamily;
            Color = color;
            ImageFormat = imageFormat;
            ImageName = imageName;
        }
    }
}