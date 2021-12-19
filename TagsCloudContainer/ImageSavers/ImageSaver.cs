using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudContainer.ImageSavers
{
    public class ImageSaver : IImageSaver
    {
        private static readonly Dictionary<string, ImageFormat> SupportedImageFormats =
            new Dictionary<string, ImageFormat>()
            {
                [".png"] = ImageFormat.Png,
                [".bmp"] = ImageFormat.Bmp,
                [".jpg"] = ImageFormat.Jpeg,
                [".gif"] = ImageFormat.Gif,
            };

        public Result<None> Save(Bitmap bitmap, string imagePath)
        {
            var invalidPathChars = Path.GetInvalidPathChars();
            if (imagePath.Any(invalidPathChars.Contains))
            {
                return Result.Fail<None>("Image path contains characters that are not allowed in path names");
            }

            var imageExtension = Path.GetExtension(imagePath);
            
            return SupportedImageFormats.TryGetValue(imageExtension, out var imageFormat)
                ? Result.OfAction(() => bitmap.Save($"{imagePath}", imageFormat))
                : Result.Fail<None>($"{imageExtension} format is not supported");
        }
    }
}