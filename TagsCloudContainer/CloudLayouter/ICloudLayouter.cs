using System.Drawing;

namespace TagsCloudContainer.CloudLayouter
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}