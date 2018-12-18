using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ResultOf;


namespace TagsCloudVisualization.ImageSaving
{
    public class StandardImageSaver : IImageSaver
    {
        private static readonly Dictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>
        {
            { "png", ImageFormat.Png },
            { "jpg", ImageFormat.Jpeg },
            { "jpeg", ImageFormat.Jpeg },
            { "bmp", ImageFormat.Bmp },
            { "gif", ImageFormat.Gif },
            { "icon", ImageFormat.Icon }
        };

        public Result<None> SaveImage(Image image, string path)
        {
            return Result.Of(() => path.ExtractFileExtension())
                .Then(extension => image.Save(path, ImageFormats[extension]));
        }

        public string[] SupportedTypes()
        {
            return ImageFormats.Keys.ToArray();
        }
    }
}
