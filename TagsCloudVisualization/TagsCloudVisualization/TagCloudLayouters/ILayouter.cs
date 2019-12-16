using System.Drawing;
using TagsCloudVisualization.Results;

namespace TagsCloudVisualization.TagCloudLayouters
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}
