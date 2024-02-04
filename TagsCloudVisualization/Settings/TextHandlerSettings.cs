using Results;

namespace TagsCloudVisualization.Settings;

public class TextHandlerSettings : ISettings
{
    public string PathToBoringWords { get; }
    public string PathToText { get; }

    public TextHandlerSettings(string pathToBoringWords, string pathToText)
    {
        PathToBoringWords = pathToBoringWords;
        PathToText = pathToText;
    }

    public Result<bool> Check()
    {
        if (!File.Exists(PathToBoringWords))
            return Result.Fail<bool>($"Cant't find file with this path {Path.GetFullPath(PathToBoringWords)}");
        if (!File.Exists(PathToText))
            return Result.Fail<bool>($"Cant't find file with this path {Path.GetFullPath(PathToText)}");
        return Result.Ok(true);
    }
}