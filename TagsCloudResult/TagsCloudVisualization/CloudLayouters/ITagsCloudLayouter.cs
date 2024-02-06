using System.Drawing;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.CloudLayouters;

public interface ITagsCloudLayouter : IDisposable
{
    Point Center { get; }
    IEnumerable<Rectangle> Rectangles { get; }
    Result<Rectangle> PutNextRectangle(Size rectangleSize);
}
