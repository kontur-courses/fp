using SixLabors.ImageSharp;

namespace TagsCloudContainer.Visualizers;

public interface ICloudVisualizer
{
    public Result<Image> DrawImage(ITagCloud cloud);
}