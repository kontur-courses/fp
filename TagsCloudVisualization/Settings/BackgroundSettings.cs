using Results;
using System.Drawing;

namespace TagsCloudVisualization.Settings;

public class BackgroundSettings : ISettings
{
    public string BackgroundColor { get; }
    
    public BackgroundSettings(string backgroundColor) 
    {
        BackgroundColor = backgroundColor;
    }

    public Result<bool> Check()
    {
        if (Enum.IsDefined(typeof(KnownColor), BackgroundColor))
            return Result.Ok(true);
        return Result.Fail<bool>($"Can't find color with name {BackgroundColor} for background");
    }
}