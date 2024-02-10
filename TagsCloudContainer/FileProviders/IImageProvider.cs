using SixLabors.ImageSharp;

namespace TagsCloudContainer.FileProviders;

public interface IImageProvider
{
    public Result SaveImage(Image image, string filePath);
}