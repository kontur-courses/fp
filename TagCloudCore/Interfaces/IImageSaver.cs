using System.Drawing;

namespace TagCloudCore.Interfaces;

public interface IImageSaver
{
    public string SupportedExtension { get; }
    void SaveImage(Image image);
}