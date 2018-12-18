using System.Drawing;

namespace TagsCloudContainer.Layouter
{
    public interface ITagCloudLayouter
    {
        Point Center { get; }
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}