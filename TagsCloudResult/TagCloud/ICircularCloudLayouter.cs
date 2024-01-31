using System.Drawing;

namespace TagsCloudResult.TagCloud;

public interface ICircularCloudLayouter
{
    Rectangle PutNextRectangle(Size rectangleSize);
}