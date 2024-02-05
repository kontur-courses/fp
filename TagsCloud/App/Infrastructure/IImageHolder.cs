using System.Drawing;
using TagsCloud.App.Settings;

namespace TagsCloud.App.Infrastructure;

public interface IImageHolder
{
    Result<Size> GetImageSize();
    Result<Graphics> StartDrawing();
    void UpdateUi();
    Result<None> RecreateImage(ImageSettings settings);
    Result<None> SaveImage(string fileName);
}