using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface ICloudLayouter 
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize, List<Rectangle> containter);
    }
}