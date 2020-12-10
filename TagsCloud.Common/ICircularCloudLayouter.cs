using System.Drawing;
using TagsCloud.ResultPattern;

namespace TagsCloud.Common
{
    public interface ICircularCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}