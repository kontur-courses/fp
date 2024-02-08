using System.Drawing;

public interface ITagCloudLayouter
{
    Result<Rectangle> PutNextRectangle(Size size);
}
