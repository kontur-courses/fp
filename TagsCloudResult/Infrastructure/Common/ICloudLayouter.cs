using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.Infrastructure.Common
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
        HashSet<Rectangle> Centering();
    }
}