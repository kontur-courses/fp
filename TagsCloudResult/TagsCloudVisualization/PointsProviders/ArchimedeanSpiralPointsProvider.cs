using System.Drawing;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.PointsProviders;

public class ArchimedeanSpiralPointsProvider : IPointsProvider
{
    private readonly ArchimedeanSpiralSettings settings;
    public Point Start => settings.Center;

    public ArchimedeanSpiralPointsProvider(ArchimedeanSpiralSettings settings)
    {
        this.settings = settings;
    }

    public Result<IEnumerable<Point>> GetPoints()
    {
        return settings.Validate()
            .Then(CalculatePoints);
    }

    private static IEnumerable<Point> CalculatePoints(ArchimedeanSpiralSettings settings)
    {
        for (var angle = 0d; ; angle += settings.DeltaAngle)
        {
            var point = settings.Center;
            point.Offset(PolarToCartesian(settings.Distance * angle, angle));

            yield return point;
        }
    }

    public static Point PolarToCartesian(double distance, double angle)
    {
        return new Point((int)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)));
    }
}
