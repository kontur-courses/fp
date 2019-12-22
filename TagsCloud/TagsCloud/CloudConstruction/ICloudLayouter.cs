using System.Drawing;
using TagsCloud.ErrorHandler;

namespace TagsCloud.CloudConstruction
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}