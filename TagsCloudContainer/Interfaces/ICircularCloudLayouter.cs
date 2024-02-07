using System.Drawing;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.Interfaces
{
    public interface ICircularCloudLayouter
    {
        Result<Point> CloudCenter { get; init; }
        IList<Rectangle> Rectangles { get; init; }
        Result<Rectangle> PutNextRectangle(string word, Font font);
    }
}
