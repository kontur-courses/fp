using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization.Layouter
{
    public interface ICloudLayouter
    {
        int Radius { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}