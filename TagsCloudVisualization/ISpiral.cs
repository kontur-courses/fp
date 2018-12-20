using System.Drawing;
using ResultOfTask;

namespace TagsCloudVisualization
{
    public interface ISpiral
    {
        Point Center { get; }
        Result<Rectangle> GetRectangleInNextLocation(Size rectangleSize);
    }
}