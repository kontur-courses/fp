using System.Collections.Generic;
using System.Drawing;
using ResultPattern;

namespace TagsCloudGenerator.CloudPrepossessing
{
    public interface ITagsPrepossessing
    {
        Point Center { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
        IReadOnlyList<Rectangle> GetRectangles();
    }
}