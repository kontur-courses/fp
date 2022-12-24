using System.Drawing;

namespace CloudLayout.Interfaces;

public interface ISpiralDrawer
{
    IEnumerable<PointF> GetSpiralPoints(Point center);
}