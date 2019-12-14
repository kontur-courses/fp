using System.Drawing;

namespace TagsCloudResult.CloudVisualizers.ImageSaving
{
    public interface IImageSaver
    {
        void Save(Bitmap bitmap);
    }
}