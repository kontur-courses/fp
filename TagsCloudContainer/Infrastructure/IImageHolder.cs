using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.Infrastructure
{
    public interface IImageHolder
    {
        Result<Size> GetImageSize();
        Graphics StartDrawing();
        void UpdateUi();
        void RecreateImage(ImageSettings settings);
        void SaveImage(string fileName);
    }
}