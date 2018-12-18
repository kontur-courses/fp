using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagCloud.Data;
using TagCloud.Reader;

namespace TagCloud.Saver
{
    public class FileImageSaver : IImageSaver
    {
        public static readonly Dictionary<string, ImageFormat> Formats = new Dictionary<string, ImageFormat>
        {
            ["png"] = ImageFormat.Png,
            ["jpg"] = ImageFormat.Jpeg,
            ["bmp"] = ImageFormat.Bmp
        };

        public Result<None> Save(Image image, string fileName)
        {
            var format = TextFileReader.GetFormat(fileName);
            return Result.OfAction(() => image.Save(fileName, Formats[format]), $"Unknown file format {format}");
        }
    }
}