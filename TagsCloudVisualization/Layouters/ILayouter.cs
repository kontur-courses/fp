using System.Drawing;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.Layouters
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}