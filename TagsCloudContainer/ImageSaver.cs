using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ResultOf;

namespace TagsCloudContainer
{
    public class ImageSaver : IImageSaver
    {
        private readonly ImageFormat imageFormat;

        public ImageSaver(ImageFormat imageFormat)
        {
            this.imageFormat = imageFormat;
        }

        public Result<string> Save(Bitmap bitmap)
        {
            return Result.Of(() =>
            {
                var path = Directory.GetCurrentDirectory();
                var fullname = Path.Combine(path, $"tagCloud.{imageFormat.ToString().ToLower()}");
                bitmap.Save(fullname, imageFormat);
                return fullname;
            });
        }
    }
}