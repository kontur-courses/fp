using Results;
using System.Drawing;

namespace TagsCloudVisualization.Settings;

public class BackgroundSettings
{
    public Result<Color> BackgroundColor { get; }
    
    public BackgroundSettings(string backgroundColor) 
    {
        if (Enum.IsDefined(typeof(KnownColor), backgroundColor))
            BackgroundColor = Result.Ok(Color.FromName(backgroundColor));
        else
            BackgroundColor = Result.Fail<Color>($"Can't find color with name {backgroundColor} for background");
    }
}