using System.Drawing;
using ResultLogic;

namespace TagCloud.TagCloudVisualisation.TagCloudLayouter
{
    public interface ITagCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}