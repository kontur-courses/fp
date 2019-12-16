using System.Drawing;
using TextConfiguration;

namespace TagsCloudLayout.CloudLayouters
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}
