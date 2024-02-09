using System.Drawing;

namespace TagCloudResult.Layouter
{
    public interface ILayouter
    {
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}
