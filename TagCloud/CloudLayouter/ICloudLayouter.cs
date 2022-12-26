using System.Drawing;
using TagCloud.ResultMonade;

namespace TagCloud.CloudLayouter
{
    public interface ICloudLayouter
    {
        Point CloudCenter { get; }

        Result<Rectangle> PutNextRectangle(Size rectangleSize);

        Size GetCloudSize();
    }
}
