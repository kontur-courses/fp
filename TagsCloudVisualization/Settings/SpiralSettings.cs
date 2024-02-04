using Results;

namespace TagsCloudVisualization.Settings;

public class SpiralSettings : ISettings
{
    public double DeltaAngle { get; }
    public double DeltaRadius { get; }

    public SpiralSettings(double deltaAngle, double deltaRadius)
    {
        DeltaAngle = deltaAngle;
        DeltaRadius = deltaRadius;
    }

    public Result<bool> Check()
    {
        if (DeltaRadius <= 0)
            return Result.Fail<bool>($"Delta radius must be positive, but {DeltaRadius}");
        if (DeltaAngle <= 0)
            return Result.Fail<bool>($"Delta angle must be positive, but {DeltaAngle}");
        return Result.Ok(true);
    }
}