using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud.Interfaces;
using TagsCloud.ErrorHandling;

namespace TagsCloud.ImageSavers
{
    public class ImageSaver : IImageSaver
    {
        public Result<None> SaveImage(Image image, string path, ImageFormat formatResult)
        {
            return Result.OfAction(() => image.Save(path, formatResult),
                $"Failed to save iamge to {path}");
        }
    }
}
