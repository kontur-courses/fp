using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloud.Graphics
{
    public class ImageSettings
    {
        public readonly Color Color;
        public readonly FontFamily FontFamily;
        public readonly ImageFormat ImageFormat;
        public readonly string ImageName;
        public readonly Size ImageSize;

        public ImageSettings(Size imageSize, FontFamily fontFamily, Color color, ImageFormat imageFormat,
            string imageName)
        {
            ImageSize = imageSize;
            FontFamily = fontFamily;
            Color = color;
            ImageFormat = imageFormat;
            ImageName = imageName;
        }
    }
}