using System.Drawing;

namespace TagCloudResult.Curves;

public class ArchimedeanSpiral : ICurve
{
    public const string Name = "spiral";

    public ArchimedeanSpiral(double startRadius = 0, double extendRatio = 0.25)
    {
        StartRadius = (startRadius < 0) ? 0 : startRadius;
        ExtendRatio = (extendRatio <= 0) ? 0.25 : extendRatio;
    }

    public double StartRadius { get; }
    public double ExtendRatio { get; }

    public Point GetPoint(double angle)
    {
        var radius = StartRadius + ExtendRatio * angle;
        var x = Convert.ToInt32(radius * Math.Cos(angle));
        var y = Convert.ToInt32(radius * Math.Sin(angle));
        return new Point(x, y);
    }
}