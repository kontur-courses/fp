using System.Drawing;

namespace TagsCloudResult.Infrastructure.Common
{
    public class ImageSetting
    {
        public int Height { get; }
        public int Width { get; }
        public string Format { get; }
        public string Name { get;  }
        
        public string BackGround { get; }

        public ImageSetting(int height, int width, string backGroundColor, string format, string name)
        {
            Height = height;
            Width = width;
            BackGround = backGroundColor;
            Format = format;
            Name = name;
        }
    }
}