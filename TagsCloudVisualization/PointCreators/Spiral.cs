using Results;
using System.Drawing;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.PointCreators;

public class Spiral : IPointCreator
{
    private readonly Result<Point> center;
    private readonly Result<double> deltaAngle;
    private readonly Result<double> deltaRadius;

    public Spiral(ImageSettings imageSettings, SpiralSettings spiralSettings)
    {
        deltaAngle = spiralSettings.DeltaAngle;
        if (!imageSettings.Width.IsSuccess || !imageSettings.Height.IsSuccess)
            center = Result.Fail<Point>($"{imageSettings.Width.Error}. {imageSettings.Height.Error}");
        else
            center = new Point(imageSettings.Width.Value / 2, imageSettings.Height.Value / 2);
        deltaAngle = spiralSettings.DeltaAngle;
        deltaRadius = spiralSettings.DeltaRadius;
    }

    public IEnumerable<Result<Point>> GetPoints()
    {
        if (!deltaRadius.IsSuccess)
        {
            yield return Result.Fail<Point>(deltaRadius.Error);
            yield break;
        }
        if (!deltaAngle.IsSuccess)
        {
            yield return Result.Fail<Point>(deltaAngle.Error);
            yield break;
        }
        if (!center.IsSuccess)
        {
            yield return Result.Fail<Point>(center.Error);
            yield break;
        }
        var angle = 0.0;
        var radius = 0.0;
        while (true)
        {
            var point = ConvertFromPolarCoordinates(angle, radius);
            point.Offset(center.Value);
            yield return point;
            angle += deltaAngle.Value;
            radius += deltaRadius.Value;
        }
    }

    public static Point ConvertFromPolarCoordinates(double angle, double radius)
    {
        var x = (int)Math.Ceiling(Math.Cos(angle) * radius);
        var y = (int)Math.Ceiling(Math.Sin(angle) * radius);
        return new Point(x, y);
    }
}