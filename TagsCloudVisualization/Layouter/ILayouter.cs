using System.Drawing;
using ResultMonad;

namespace TagsCloudVisualization.Layouter
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}