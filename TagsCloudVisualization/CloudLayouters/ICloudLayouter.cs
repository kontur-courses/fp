using Results;
using System.Drawing;

namespace TagsCloudVisualization.CloudLayouters;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}
