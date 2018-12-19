using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Settings
{
    public interface IImageHolder
    {
        Result<Size> GetImageSize();
        Result<Graphics> StartDrawing();
        Result<None> RecreateImage(ImageSettings settings);
        Result<None> SaveImage(string fileName);
    }
}