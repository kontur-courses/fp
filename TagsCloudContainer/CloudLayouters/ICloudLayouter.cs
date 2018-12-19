using System.Drawing;

namespace TagsCloudContainer.CloudLayouters
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}