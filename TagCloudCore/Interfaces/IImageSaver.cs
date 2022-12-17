using System.Drawing;
using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces;

public interface IImageSaver
{
    public string SupportedExtension { get; }
    Result<None> SaveImage(Image image);
}