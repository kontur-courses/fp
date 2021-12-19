using System.Drawing;

namespace TagsCloudContainer.ImageSavers
{
    public interface IImageSaver
    {
        Result<None> Save(Bitmap bitmap, string imagePath);
    }
}