using System.Drawing;

namespace TagsCloudContainer.CircularCloudLayouters
{
    public interface ICircularCloudLayouter
    {
        Result<Rectangle> PutNextRectangle(Size size);
    }
}