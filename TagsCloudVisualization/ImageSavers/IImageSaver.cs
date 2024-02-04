using Results;
using System.Drawing;

namespace TagsCloudVisualization.ImageSavers;

public interface IImageSaver
{
    Result<None> SaveImage(Bitmap bitmap);
}