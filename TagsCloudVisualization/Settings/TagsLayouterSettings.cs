using Results;
using System.Drawing;

namespace TagsCloudVisualization.Settings;

public class TagsLayouterSettings
{
    public Result<FontFamily> FontFamily { get; }
    public Result<int> MinSize { get; }
    public Result<int> MaxSize { get; }

    public TagsLayouterSettings(string font, int minSize, int maxSize)
    {
        FontFamily = GetFontFamily(font);
        if (maxSize < minSize)
            MaxSize = Result.Fail<int>($"Max font size can't be less then min font size");
        else if (maxSize <= 0)
        {
            MaxSize = Result.Fail<int>($"Font sizes must be positive, but max size: {maxSize}");
        }
        else if (minSize <= 0)
        {
            MinSize = Result.Fail<int>($"Font sizes must be positive, but min size: {minSize}");
        }
        else
        {
            MaxSize = Result.Ok(maxSize);
            MinSize = Result.Ok(minSize);
        }
    }

    private Result<FontFamily> GetFontFamily(string fontName)
    {
        try
        {
            return Result.Ok(new FontFamily(fontName));
        }
        catch (ArgumentException)
        {
            return Result.Fail<FontFamily>($"Font with name {fontName} doesn't supported");
        }
    }
}