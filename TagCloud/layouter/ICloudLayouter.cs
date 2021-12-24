using System.Drawing;

namespace TagCloud.layouter
{
    public interface ICloudLayouter
    {
        public Result<RectangleF> PutNextRectangle(Size rectangleSize);
    }
}