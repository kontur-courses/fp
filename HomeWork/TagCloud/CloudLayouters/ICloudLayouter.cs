using System.Drawing;

namespace TagCloud.CloudLayouters
{
    public interface ICloudLayouter
    {
        public delegate ICloudLayouter Factory();
        public Point Center { get; }
        public Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}
