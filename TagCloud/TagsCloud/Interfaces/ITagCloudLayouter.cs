using System.Drawing;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITagCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}