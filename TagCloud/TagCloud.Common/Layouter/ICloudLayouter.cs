using System.Drawing;
using ResultOf;

namespace TagCloud.Common.Layouter;

public interface ICloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
    IEnumerable<Rectangle> GetRectanglesLayout();
    void ClearRectanglesLayout();
}