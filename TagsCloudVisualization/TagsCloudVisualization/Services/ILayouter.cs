using System.Drawing;
using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        void Reset();
    }
}