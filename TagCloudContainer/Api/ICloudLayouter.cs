using System.Collections.Generic;
using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface ICloudLayouter 
    {
        Rectangle PutNextRectangle(Size rectangleSize, List<Rectangle> containter);
    }
}