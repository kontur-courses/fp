using System.Drawing;
using TagCloudGenerator;

namespace TagsCloudVisualization
{
    public interface ICircularCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}