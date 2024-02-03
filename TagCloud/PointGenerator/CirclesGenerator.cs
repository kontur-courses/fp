using System.Drawing;
using TagCloud.AppSettings;

namespace TagCloud.PointGenerator;

public class CirclesGenerator : IPointGenerator
{
    private readonly Point startPoint;
    private int density;
    private const double angleShift = 0.01;
    private double currentAngle;

    public string GeneratorName => "Circular";

    public CirclesGenerator(IAppSettings settings)
    {
        startPoint = new Point(settings.CloudWidth / 2, settings.CloudHeight / 2);
        density = settings.CloudDensity * 200;
    }

    public Point GetNextPoint()
    {
        var radius = density;
        var x = (int)(Math.Cos(currentAngle) * radius);
        var y = (int)(Math.Sin(currentAngle) * radius);
        currentAngle += angleShift;

        if (currentAngle < 2 * Math.PI && currentAngle + angleShift > 2 * Math.PI)
            density += 100;

        return new Point(startPoint.X + x, startPoint.Y + y);
    }
}