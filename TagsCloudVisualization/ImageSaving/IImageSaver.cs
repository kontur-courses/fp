using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.ImageSaving
{
    public interface IImageSaver
    {
        Result<None> SaveImage(Image image, string path);
        string[] SupportedTypes();
    }
}
