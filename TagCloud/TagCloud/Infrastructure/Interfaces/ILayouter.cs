using ResultOF;
using System.Drawing;

namespace TagCloud
{
    public interface ILayouter
    {
        Result<RectangleF> PutNextRectangle(SizeF rectangleSize);
        void Reset();
    }
}
