using System.Drawing;
using TagsCloud;

namespace TagCloud.TagCloudVisualisation.TagCloudLayouter
{
    public interface ITagCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}