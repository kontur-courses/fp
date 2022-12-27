using System.Drawing;
using TagCloud.ResultImplementation;

namespace TagCloud.CloudLayouter
{
    public interface ICloudLayouter<T>
    {
        Result<T> PutNextRectangle(Size size);
    }
}