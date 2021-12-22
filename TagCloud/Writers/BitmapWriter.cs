using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using TagCloud.ResultMonad;

namespace TagCloud.Writers
{
    public class BitmapWriter : IFileWriter
    {
        public Result<None> Write(Bitmap bitmap, string filename, string extension, string targetDirectory)
        {
            var format = GetImageFormatByExtension(extension);
            if (format == null)
                return Result.Fail<None>("Wrong output file format");
            var env = targetDirectory + "\\";
            return Result.OfAction(() => bitmap.Save(env + filename, format));
        }

        private ImageFormat GetImageFormatByExtension(string extension)
        {
            var type = typeof(ImageFormat);
            var property = type
                .GetProperty(extension, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (property == null)
                return null;
            return (ImageFormat)property.GetValue(extension, null);
        }
    }
}
