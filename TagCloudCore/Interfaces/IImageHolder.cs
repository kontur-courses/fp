using System.Drawing;

namespace TagCloudCore.Interfaces;

public interface IImageHolder
{
    Size GetImageSize();
    Graphics StartDrawing();
    void UpdateUi();
    void RecreateImage();
    void SaveImage();
}