using Results;

namespace TagsCloudVisualization.Settings;

public class SpiralSettings
{
    public Result<double> DeltaAngle { get; }
    public Result<double> DeltaRadius { get; }

    public SpiralSettings(double deltaAngle, double deltaRadius) 
    {
        if (deltaRadius <= 0)
            DeltaRadius = Result.Fail<double>($"Delta radius must be positive, but {deltaRadius}");
        else
            DeltaRadius = Result.Ok(deltaRadius);
        if (deltaAngle <= 0)
            DeltaAngle = Result.Fail<double>($"Delta angle must be positive, but {deltaAngle}");
        else
            DeltaAngle = Result.Ok(deltaAngle);
    }
}
