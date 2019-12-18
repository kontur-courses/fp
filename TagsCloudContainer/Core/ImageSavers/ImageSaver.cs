using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ResultOf;

namespace TagsCloudContainer.Core.ImageSavers
{
    class ImageSaver : IImageSaver
    {
        private readonly Dictionary<string, ImageFormat> formats;

        public ImageSaver()
        {
            formats = new Dictionary<string, ImageFormat>
            {
                ["jpeg"] = ImageFormat.Jpeg,
                ["jpg"] = ImageFormat.Jpeg,
                ["png"] = ImageFormat.Png,
                ["gif"] = ImageFormat.Gif,
                ["bmp"] = ImageFormat.Bmp,
                ["tif"] = ImageFormat.Tiff
            };
        }

        public Result<None> Save(string pathImage, Bitmap bitmap, string format)
        {
            return formats.ContainsKey(format)
                ? Result.OfAction(() =>
                {
                    bitmap.Save(pathImage, formats[format]);
                    Console.WriteLine($"Tag cloud visualization saved to file {pathImage}");
                }, "Не удалось сохранить файл")
                : Result.Fail<None>(
                    $"Неподдерживаемый формат\nПоддерживаются {string.Join(", ", formats.Keys)}");
        }
    }
}