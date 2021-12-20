using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer
{
    public static class FileSaver
    {
        private static readonly Dictionary<string, ImageFormat> Formats = new()
        {
            {"jpg", ImageFormat.Jpeg}, {"bmp", ImageFormat.Bmp}, {"png", ImageFormat.Png}
        };

        public static Result<None> Save(Bitmap bitmap, string fileName, string directory, string format)
        {
            if (!Formats.ContainsKey(format))
            {
                return Result.Fail<None>("Unexpected format");
            }

            return Result.Ok(new None()).ThenDo(_ => bitmap.Save(directory + fileName + "." + format, Formats[format]));
        }
    }
}