using System.Drawing;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.Layouter;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
    Rectangle[] GetLayout();
}