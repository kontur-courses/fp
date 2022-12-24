using System.Drawing;
using CloudLayout.Interfaces;

namespace CloudLayout;

public class SpiralDrawer : ISpiralDrawer
{
    public IEnumerable<PointF> GetSpiralPoints(Point center)
    {
        double angle = 0;
        double adjustAngle = 1;
        while (center.X + Math.Cos(angle) * angle / 0.95 > 0
               && center.Y + Math.Sin(angle) * angle / 0.95 > 0)
        {
            yield return new PointF((int)(center.X + Math.Cos(angle) * angle / 0.95),
                (int)(center.Y + Math.Sin(angle) * angle / 0.95));
            var coilCount = (int)(angle / (2 * Math.PI) + 1);
            adjustAngle = adjustAngle <= 0.017 ? 0.017 : Math.PI / 4 / coilCount;
            angle += adjustAngle;
        }
    }
}