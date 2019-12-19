using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Functional;

namespace TagsCloudContainer.Core.Layouters
{
    public interface IRectangleLayouter
    {
        IEnumerable<Rectangle> Rectangles { get; }

        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}