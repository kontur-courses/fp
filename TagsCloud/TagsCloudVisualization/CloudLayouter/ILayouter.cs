using System.Drawing;
using ResultMonad;

namespace TagsCloudVisualization.CloudLayouter
{
    public interface ILayouter
    {
        public Result<Rectangle> PutNextRectangle(PositiveSize rectangleSize);
    }
}