using System.Drawing;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;

namespace TagsCloudVisualization.PointsProviders;

public class ArchimedeanSpiralSettings : ISettings<ArchimedeanSpiralSettings>
{
    public Point Center { get; init; } = new(500, 500);
    public double DeltaAngle { get; init; } = 5 * Math.PI / 180;
    public double Distance { get; init; } = 2;
    
    public Result<ArchimedeanSpiralSettings> Validate()
    {
        return ValidateArguments(this);
    }
    
    private static Result<ArchimedeanSpiralSettings> ValidateArguments(ArchimedeanSpiralSettings settings)
    {
        return Result.Validate(settings, x => x.DeltaAngle != 0 && x.Distance != 0,
            Resources.ArchimedeanSpiralSettings_ValidateArguments_Fail);
    }
}
