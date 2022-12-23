using System.Drawing;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.InfrastructureUI
{
    public interface IImageHolder
    {
        Result<Size> GetImageSize();
        Result<Graphics> StartDrawing();

        void UpdateUi();

        Result<None> RecreateImage(ImageSettings settings);

        Result<None> SaveImage(string fileName);

        Result<None> FailIfNotInitialized();
    }
}