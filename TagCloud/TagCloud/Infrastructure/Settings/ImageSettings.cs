using ResultOF;
using System;
using System.Drawing;

namespace TagCloud
{
    public class ImageSettings
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public PointF CloudCenter => new PointF(CenterX, CenterY);

        public ImageSettings(int height, int width, float centerX, float centerY)
        {
            CenterX = centerX;
            CenterY = centerY;
            Height = height;
            Width = width;
        }

        public static ImageSettings GetDefaultSettings() =>
            new ImageSettings(1000, 1000, 500, 500);

        public Result<None> ValidateImageSettings()
        {
            return Height > 0 && Width > 0 && CenterX > 0 && CenterY > 0 ?
                Result.Ok() : Result.Fail<None>("Invalid image settings");
        }
    }
}
