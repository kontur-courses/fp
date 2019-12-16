using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using TextConfiguration;

namespace TagsCloudVisualization
{
    public static class ImageUtilities
    {
        public static Result<ImageFormat> GetFormatFromString(string formatName)
        {
            var imageFormatProperty = typeof(ImageFormat)
                .GetProperty(formatName, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (imageFormatProperty is null)
                return Result.Fail<ImageFormat>($"Cannot convert string. Unknown image format: {formatName}");
            return imageFormatProperty.GetValue(null) as ImageFormat;
        }

        public static Result<None> SaveImage(Bitmap image, ImageFormat format, string filePath)
        {
            if (filePath is null || Directory.Exists(filePath))
                return Result.Fail<None>($"Cannot save image. Incorrect file path: {filePath}");
            image.Save(filePath, format);
            return Result.Ok();
        }
    }
}
