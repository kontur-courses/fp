using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Common.ImageWriters
{
    public class ImageWriter : IImageWriter
    {
        public Result<None> Save(Bitmap bitmap, string path)
        {
            return Result.OfAction(() =>
            {
                bitmap.Save(path, GetImageFormat(path));
                bitmap.Dispose();
            }).RefineError("Невозможно сохранить файл:");
        }

        private static ImageFormat GetImageFormat(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            if (string.IsNullOrEmpty(extension))
                return ImageFormat.Bmp;

            switch (extension)
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new ArgumentException($"Неверный формат изображения '{extension}'.");
            }
        }
    }
}