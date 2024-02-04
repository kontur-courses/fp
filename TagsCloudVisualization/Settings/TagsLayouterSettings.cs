using Results;
using System.Drawing;

namespace TagsCloudVisualization.Settings;

public class TagsLayouterSettings : ISettings
{
    public string FontFamily { get; }
    public int MinSize { get; }
    public int MaxSize { get; }

    public TagsLayouterSettings(string font, int minSize, int maxSize)
    {
        FontFamily = font;
        MinSize = minSize;
        MaxSize = maxSize;
    }

    private bool IsCanGetFontFamily(string fontName)
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

    public Result<bool> Check()
    {
        if (!IsCanGetFontFamily(FontFamily))
            return Result.Fail<bool>($"Font with name {FontFamily} doesn't supported");
        else if (MaxSize < MinSize)
            return Result.Fail<bool>($"Max font size can't be less then min font size");
        else if (MaxSize <= 0)
            return Result.Fail<bool>($"Font sizes must be positive, but max size: {MaxSize}");
        else if (MinSize <= 0)
            return Result.Fail<bool>($"Font sizes must be positive, but min size: {MinSize}");
        return Result.Ok(true);
    }
}