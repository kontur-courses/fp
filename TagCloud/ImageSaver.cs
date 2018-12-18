using System.Drawing;
using System.Drawing.Imaging;
using TagCloud.Interfaces;
using TagCloud.Result;

namespace TagCloud
{
    public class ImageSaver : IImageSaver
    {
        public Result<None> Save(Image image, string path)
        {
            return Result.Result.OfAction(() => image.Save(path, ImageFormat.Png));
        }
    }
}