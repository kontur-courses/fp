using ResultPatterLibrary;
using System.Drawing;

namespace TagsCloudVisualization.TagCloudLayouters
{
    public interface ILayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}
