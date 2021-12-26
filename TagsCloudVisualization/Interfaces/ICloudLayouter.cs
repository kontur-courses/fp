using System.Drawing;

namespace TagsCloudVisualization.Interfaces
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
        Result<Point> GetCenter();
    }
}