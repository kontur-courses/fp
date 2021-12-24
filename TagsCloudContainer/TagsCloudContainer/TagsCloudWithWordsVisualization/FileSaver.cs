using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public static class FileSaver
    {
        private static readonly Dictionary<string, ImageFormat> Formats = new()
        {
            {".jpg", ImageFormat.Jpeg}, {".bmp", ImageFormat.Bmp}, {".png", ImageFormat.Png}
        };

        public static Result<None> Save(Bitmap bitmap, string path)
        {
            var format = Path.GetExtension(path);
            return !Formats.ContainsKey(format)
                ? Result.Fail<None>("Unexpected format")
                : Result.Ok(new None()).ThenDo(_ => bitmap.Save(path, Formats[format]));
        }
    }
}