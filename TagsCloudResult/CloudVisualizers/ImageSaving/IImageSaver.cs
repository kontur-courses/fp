using System.Drawing;

namespace TagsCloudResult.CloudVisualizers.ImageSaving
{
    public interface IImageSaver
    {
        Result<None> Save(Bitmap bitmap);
    }
}