using System.Drawing;
using TagCloud.Infrastructure;

namespace TagCloud.ImageGenerator;

public interface IImageGenerator
{
    public Result<Image> GenerateImage(Tag[] tags);
}