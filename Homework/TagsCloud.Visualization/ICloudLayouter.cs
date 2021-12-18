using System.Drawing;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}