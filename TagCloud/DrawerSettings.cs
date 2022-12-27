using System.Drawing;
using FluentResults;

namespace TagCloud;

public class DrawerSettings
{
    private DrawerSettings(Size imageSize, int minFontSize, int maxFontSize, Color textColor, Color backgroundColor,
        FontFamily fontFamily)
    {
        ImageSize = imageSize;
        MinFontSize = minFontSize;
        MaxFontSize = maxFontSize;
        TextColor = textColor;
        BackgroundColor = backgroundColor;
        FontFamily = fontFamily;
    }

    public Size ImageSize { get; }
    public int MinFontSize { get; }
    public int MaxFontSize { get; }

    public Color TextColor { get; }
    public Color BackgroundColor { get; }
    public FontFamily FontFamily { get; }

    public static Result<DrawerSettings> Create(
        Size imageSize,
        int minFontSize = 10, int maxFontSize = 50,
        string textColorName = "Black",
        string backgroundColorName = "White",
        string fontFamilyName = "Arial")
    {
        var errors = new List<Error>();

        if (!imageSize.IsPositive())
            errors.Add(new Error(
                $"Width and height of the image must be positive, but {imageSize}."));

        if (minFontSize <= 0 || minFontSize > maxFontSize)
            errors.Add(new Error(
                $"MinFontSize should be greater than 0 and less than MaxFontSize, but {minFontSize}."));

        if (maxFontSize <= 0 || maxFontSize < minFontSize)
            errors.Add(new Error(
                $"MaxFontSize should be greater than 0 and MinFontSize, but {maxFontSize}."));

        CheckColor(textColorName, errors, out var textColor);

        CheckColor(backgroundColorName, errors, out var backgroundColor);

        CheckFontFamily(fontFamilyName, errors, out var fontFamily);

        if (errors.Any())
            return Result.Fail(errors);

        return new DrawerSettings(imageSize, minFontSize, maxFontSize, textColor, backgroundColor, fontFamily);
    }

    private static void CheckColor(string colorName, ICollection<Error> errors, out Color color)
    {
        color = Color.FromName(colorName);
        if (!color.IsKnownColor)
            errors.Add(new Error($"Unknown color '{colorName}'."));
    }

    private static void CheckFontFamily(string fontFamilyName, ICollection<Error> errors, out FontFamily fontFamily)
    {
        try
        {
            fontFamily = new FontFamily(fontFamilyName);
        }
        catch (ArgumentException e)
        {
            errors.Add(new Error(e.Message));
            fontFamily = default!;
        }
    }
}