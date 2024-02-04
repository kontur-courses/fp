using Results;
using System.Drawing;

namespace TagsCloudVisualization.CloudLayouters;

public interface ICloudLayouter
{
    public IList<Rectangle> Rectangles { get; }
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}
