using SixLabors.ImageSharp;

namespace TagsCloudContainer.FileProviders;

public interface IImageProvider
{
    public Result<None> SaveImage(Image image, string filePath);
}