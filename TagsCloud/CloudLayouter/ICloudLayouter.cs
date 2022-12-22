using System.Drawing;
using TagCloud.ResultImplementation;

namespace TagCloud.CloudLayouter
{
    public interface ICloudLayouter<out T>
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}