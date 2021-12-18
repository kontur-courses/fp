using System.Drawing;
using ResultMonad;
using TagsCloud.Utils;

namespace TagsCloudVisualization.CloudLayouter
{
    public interface ILayouter
    {
        public Result<Rectangle> PutNextRectangle(PositiveSize rectangleSize);
    }
}