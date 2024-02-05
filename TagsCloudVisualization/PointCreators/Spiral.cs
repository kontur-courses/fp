using System.Drawing;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.PointCreators;

public class Spiral : IPointCreator
{
    private readonly Point center;
    private readonly double deltaAngle;
    private readonly double deltaRadius;
    private double angle;
    private double radius;

    public Spiral(ImageSettings imageSettings, SpiralSettings spiralSettings)
    {
        center = new Point(imageSettings.Width / 2, imageSettings.Height / 2);
        if (spiralSettings.DeltaAngle <= 0 || spiralSettings.DeltaRadius <= 0)
            throw new ArgumentException("Delta angle and delta radius must be positive");
        deltaAngle = spiralSettings.DeltaAngle;
        deltaRadius = spiralSettings.DeltaRadius;
    }

    public Point GetNextPoint()
    {
        var point = ConvertFromPolarCoordinates(angle, radius);
        point.Offset(center);
        angle += deltaAngle;
        radius += deltaRadius;
        return point;
    }

    public static Point ConvertFromPolarCoordinates(double angle, double radius)
    {
        var x = (int)Math.Ceiling(Math.Cos(angle) * radius);
        var y = (int)Math.Ceiling(Math.Sin(angle) * radius);
        return new Point(x, y);
    }
}