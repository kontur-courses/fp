using System.Drawing;

namespace TagsCloudGenerator.Saver
{
    public interface IImageSaver
    {
        Result<None> Save(Bitmap bitmap);
    }
}