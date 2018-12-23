using System.Drawing;
using ResultOf;

namespace CloodLayouter.Infrastructer
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}