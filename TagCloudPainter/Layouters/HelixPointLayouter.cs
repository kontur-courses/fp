using System.Drawing;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Layouters;

public class HelixPointLayouter
{
    private readonly double angleStep;
    private readonly Point center;
    private readonly double radiusStep;
    private double angle;
    private double radius;

    public HelixPointLayouter(Point center, double angleStep, double radiusStep)
    {
        this.center = center;
        this.angleStep = angleStep;
        this.radiusStep = radiusStep;
    }

    public Result<Point> GetPoint()
    {
        if (radiusStep <= 0 || angleStep == 0) return Result.Fail<Point>("radius or angel < 0");
        var x = center.X + GetX(radius, angle);
        var y = center.Y + GetY(radius, angle);
        var location = new Point(x, y);
        angle += angleStep;
        radius += radiusStep;
        return location;
    }

    private static int GetX(double r, double a)
    {
        return (int)(r * Math.Cos(a));
    }

    private static int GetY(double r, double a)
    {
        return (int)(r * Math.Sin(a));
    }
}