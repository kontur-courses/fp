using System.Drawing;

namespace TagsCloudContainer.CloudLayouters;

public interface ICloudLayouter
{
    public Result<Rectangle> PutNextRectangle(Size rectangleSize);

    public Result UpdateCloud();
}