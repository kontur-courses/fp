using System.Drawing;

namespace TagsCloud.Layout
{
    public interface ICoordinateSequence
    {
        Point GetNextCoordinate();
    }
}