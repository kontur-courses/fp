using System.Drawing;

namespace TagCloud
{
    public interface IPlacementStrategy
    {
        Rectangle PlaceRectangle(Rectangle newRectangle, Rectangle[] existingRectangles);
    }
}
