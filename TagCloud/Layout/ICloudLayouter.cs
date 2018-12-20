using System.Drawing;

namespace TagsCloud.Layout
{
    public interface ICloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}