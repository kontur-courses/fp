using System.Drawing;
using TagCloud.AppSettings;

namespace TagCloud.PointGenerator;

public class SpiralGenerator : IPointGenerator
{
    private readonly Point startPoint;
    private readonly int spiralDensity;
    private const double angleShift = 0.1;
    private double currentAngle;

    public string GeneratorName => "Spiral";

    public SpiralGenerator(IAppSettings settings)
    {
        startPoint = new Point(settings.CloudWidth / 2, settings.CloudHeight / 2);
        spiralDensity = settings.CloudDensity;
    }

    public Point GetNextPoint()
    {
        var radius = spiralDensity * currentAngle;
        var x = (int)(Math.Cos(currentAngle) * radius);
        var y = (int)(Math.Sin(currentAngle) * radius);
        currentAngle += angleShift;

        return new Point(startPoint.X + x, startPoint.Y + y);
    }
}