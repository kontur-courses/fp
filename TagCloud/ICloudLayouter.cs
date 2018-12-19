using System.Drawing;

namespace TagsCloud
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}