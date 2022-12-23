using System.Drawing;
using ResultOf;

namespace TagsCloudVisualization
{
    public interface ICloudLayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}