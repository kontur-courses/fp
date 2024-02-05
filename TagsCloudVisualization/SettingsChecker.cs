using Results;
using System.Drawing;
using TagsCloudVisualization.CommandLine;

namespace TagsCloudVisualization;

public static class SettingsChecker
{
    public static Result<None> CheckSettings(CommandLineOptions options)
    {
        var backgroundResult = CheckBackgroundSettings(options.BackgroundColor);
        if (!backgroundResult.IsSuccess)
            return Result.Fail<None>(backgroundResult.Error);
        
        var imageResult = CheckImageSettings(options.ImageWidth, options.ImageHeight);
        if (!imageResult.IsSuccess)
            return Result.Fail<None>(imageResult.Error);

        var spiralResult = CheckSpiralSettings(options.DeltaRadius, options.DeltaAngle);
        if (!spiralResult.IsSuccess)
            return Result.Fail<None>(spiralResult.Error);

        var textHandlerResult = CheckTextHandlerSettings(options.PathToBoringWords, options.PathToText);
        if (!textHandlerResult.IsSuccess)
            return Result.Fail<None>(textHandlerResult.Error);

        var tagsLayouterResult = CheckTagsLayouterSettings(options.Font, options.MaxFontSize, options.MinFontSize);
        if (!tagsLayouterResult.IsSuccess)
            return Result.Fail<None>(tagsLayouterResult.Error);
        return Result.Ok();
    }

    private static Result<None> CheckBackgroundSettings(string backgroundColor)
    {
        if (Enum.IsDefined(typeof(KnownColor), backgroundColor))
            return Result.Ok();
        return Result.Fail<None>($"Can't find color with name {backgroundColor} for background");
    }

    private static Result<None> CheckImageSettings(int width, int height)
    {
        if (width <= 0)
            return Result.Fail<None>($"Width must be positive, but {width}");
        else if (height <= 0)
            return Result.Fail<None>($"Height must be positive, but {height}");
        return Result.Ok();
    }

    private static Result<None> CheckSpiralSettings(double deltaRadius, double deltaAngle)
    {
        if (deltaRadius <= 0)
            return Result.Fail<None>($"Delta radius must be positive, but {deltaRadius}");
        if (deltaAngle <= 0)
            return Result.Fail<None>($"Delta angle must be positive, but {deltaAngle}");
        return Result.Ok();
    }

    private static Result<None> CheckTextHandlerSettings(string pathToBoringWords, string pathToText)
    {
        if (!File.Exists(pathToBoringWords))
            return Result.Fail<None>($"Cant't find file with this path {Path.GetFullPath(pathToBoringWords)}");
        if (!File.Exists(pathToText))
            return Result.Fail<None>($"Cant't find file with this path {Path.GetFullPath(pathToText)}");
        return Result.Ok();
    }

    private static Result<None> CheckTagsLayouterSettings(string fontFamily, int maxSize, int minSize)
    {
        if (!IsCanGetFontFamily(fontFamily))
            return Result.Fail<None>($"Font with name {fontFamily} doesn't supported");
        else if (maxSize < minSize)
            return Result.Fail<None>($"Max font size can't be less then min font size");
        else if (maxSize <= 0)
            return Result.Fail<None>($"Font sizes must be positive, but max size: {maxSize}");
        else if (minSize <= 0)
            return Result.Fail<None>($"Font sizes must be positive, but min size: {minSize}");
        return Result.Ok();
    }

    private static bool IsCanGetFontFamily(string fontName)
    {
        try
        {
            new FontFamily(fontName);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}