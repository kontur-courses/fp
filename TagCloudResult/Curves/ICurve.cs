using System.Drawing;

namespace TagCloudResult.Curves;

public interface ICurve
{
    Point GetPoint(double t);
}