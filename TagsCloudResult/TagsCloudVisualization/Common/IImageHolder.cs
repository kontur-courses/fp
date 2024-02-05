using System.Drawing;

namespace TagsCloudVisualization.Common;

public interface IImageHolder
{
    Result<Size> GetImageSize();
    Result<Graphics> StartDrawing();
    Result<None> UpdateUi();
    Result<None> RecreateImage(ImageSettings settings);
    Result<None> SaveImage(string fileName);
}
