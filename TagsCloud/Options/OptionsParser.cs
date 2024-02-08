using System.Drawing;
using System.Drawing.Text;
using TagsCloud.Options;

namespace TagsCloud.ConsoleOptions;

public static class OptionsParser
{
    public static Result<LayouterOptions> ParseOptions(ConsoleArguments consoleArguments)
    {
        var layouterOptions = new LayouterOptions();
        layouterOptions.InputFile = ParseInputFileName(consoleArguments);
        layouterOptions.OutputFile = ParseOutputFileName(consoleArguments);
        layouterOptions.FontName = ParseFont(consoleArguments);
        layouterOptions.ImageSize = ParseImageSize(consoleArguments);
        layouterOptions.BackgroundColor = ParseBackgroundColor(consoleArguments);
        layouterOptions.Center = ParseLayouterCenter(consoleArguments);

        return layouterOptions;
    }

    private static string ParseInputFileName(ConsoleArguments consoleArguments)
    {
        if (string.IsNullOrEmpty(consoleArguments.InputFileName))
        {
            throw new ArgumentException();
        }

        return consoleArguments.InputFileName;
    }

    private static string ParseOutputFileName(ConsoleArguments consoleArguments)
    {
        if (string.IsNullOrEmpty(consoleArguments.OutputFileName))
        {
            throw new ArgumentException();
        }

        return consoleArguments.OutputFileName;
    }

    private static string ParseFont(ConsoleArguments consoleArguments)
    {
        var fonts = new InstalledFontCollection();
        if (fonts.Families.All(f => f.Name != consoleArguments.FontName))
        {
            throw new AggregateException();
        }

        return consoleArguments.FontName;
    }

    private static Size ParseImageSize(ConsoleArguments consoleArguments)
    {
        if (string.IsNullOrEmpty(consoleArguments.ImageWidth) || string.IsNullOrEmpty(consoleArguments.ImageHeight))
        {
            throw new AggregateException();
        }

        if (int.TryParse(consoleArguments.ImageWidth, out var w) &&
            int.TryParse(consoleArguments.ImageHeight, out var h))
        {
            return new Size(w, h);
        }

        throw new ArgumentException();
    }

    private static Point ParseLayouterCenter(ConsoleArguments consoleArguments)
    {
        if (string.IsNullOrEmpty(consoleArguments.CenterX) || string.IsNullOrEmpty(consoleArguments.CenterY))
        {
            throw new AggregateException();
        }

        if (int.TryParse(consoleArguments.CenterX, out var x) &&
            int.TryParse(consoleArguments.CenterY, out var y))
        {
            return new Point(x, y);
        }

        throw new ArgumentException();
    }

    private static Color ParseBackgroundColor(ConsoleArguments consoleArguments)
    {
        if (string.IsNullOrEmpty(consoleArguments.BackgroundColor))
        {
            throw new AggregateException();
        }

        var color = Color.FromName(consoleArguments.BackgroundColor);

        if (color.IsKnownColor)
        {
            return color;
        }

        throw new AggregateException();
    }
}