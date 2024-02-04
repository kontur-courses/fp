using System.Drawing;
using ResultLibrary;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;

namespace TagsCloudPainter.FormPointer;

public class ArchimedeanSpiralPointer : IFormPointer
{
    private readonly ICloudSettings cloudSettings;
    private readonly ISpiralPointerSettings spiralPointerSettings;
    private double сurrentDifference;

    public ArchimedeanSpiralPointer(ICloudSettings cloudSettings, ISpiralPointerSettings spiralPointerSettings)
    {
        this.cloudSettings = cloudSettings ?? throw new ArgumentNullException(nameof(cloudSettings));
        this.spiralPointerSettings =
            spiralPointerSettings ?? throw new ArgumentNullException(nameof(spiralPointerSettings));
        сurrentDifference = 0;
    }

    private double Angle => сurrentDifference * spiralPointerSettings.AngleConst;
    private double Radius => сurrentDifference * spiralPointerSettings.RadiusConst;

    public Result<Point> GetNextPoint()
    {
        if (spiralPointerSettings.Step <= 0)
            return Result.Fail<Point>("Step is not possitive");
        if (spiralPointerSettings.RadiusConst <= 0)
            return Result.Fail<Point>("RadiusConst is not possitive");
        if (spiralPointerSettings.AngleConst <= 0)
            return Result.Fail<Point>("AngleConst is not possitive");

        сurrentDifference += spiralPointerSettings.Step;
        var x = cloudSettings.CloudCenter.X + (int)(Radius * Math.Cos(Angle));
        var y = cloudSettings.CloudCenter.Y + (int)(Radius * Math.Sin(Angle));

        return Result.Of(() => new Point(x, y));
    }

    public void Reset()
    {
        сurrentDifference = 0;
    }
}