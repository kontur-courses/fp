using Results;
using System.Drawing;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.PointCreators;

public class Spiral : IPointCreator
{
    private readonly Point center;
    private readonly double deltaAngle;
    private readonly double deltaRadius;
    private readonly ImageSettings imageSettings;
    private readonly SpiralSettings spiralSettings;
    private double angle;
    private double radius;

    public Spiral(ImageSettings imageSettings, SpiralSettings spiralSettings)
    {
        this.imageSettings = imageSettings;
        this.spiralSettings = spiralSettings;
        center = new Point(imageSettings.Width / 2, imageSettings.Height / 2);
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

    public Result<bool> CheckForCorrectness()
    {
        var imageSettingsCheck = imageSettings.Check();
        if (!imageSettingsCheck.IsSuccess)
            return Result.Fail<bool>(imageSettingsCheck.Error);
        var spiralSettingsCheck = spiralSettings.Check();
        if (!spiralSettingsCheck.IsSuccess)
            return Result.Fail<bool>(spiralSettingsCheck.Error);
        return Result.Ok(true);
    }
}