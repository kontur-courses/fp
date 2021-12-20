using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudApp.ConsoleActions;

public class ImageSettingsAction : IUIAction
{
    private readonly Dictionary<string, Color> colorSelector;
    private readonly Dictionary<string, FontFamily> fontFamilySelector;
    private readonly Dictionary<string, ImageFormat> formatSelector;
    private readonly Settings settings;
    private readonly Dictionary<string, FontStyle> styleSelector;

    public ImageSettingsAction(
        Settings settings,
        Color[] colors,
        FontFamily[] fonts,
        FontStyle[] styles,
        ImageFormat[] formats)
    {
        this.settings = settings;

        colorSelector = PrepareSelector(colors);
        fontFamilySelector = PrepareSelector(fonts);
        styleSelector = PrepareSelector(styles);
        formatSelector = PrepareSelector(formats);
    }

    public string GetDescription()
    {
        return "Image settings";
    }

    public void Handle()
    {
        Console.WriteLine("1 - Palette");
        Console.WriteLine("2 - Font");
        Console.WriteLine("3 - Format");
        Console.WriteLine("4 - Size");
        Console.WriteLine("5 - Back");
        var answer = Console.ReadLine();
        Console.WriteLine();
        switch (answer)
        {
            case "1":
                PaletteKey();
                break;
            case "2":
                FontKey();
                break;
            case "3":
                FormatKey();
                break;
            case "4":
                SizeKey();
                break;
            default:
                return;
        }
    }

    private Dictionary<string, T> PrepareSelector<T>(T[] elements)
    {
        var elementSelector = new Dictionary<string, T>();
        for (var i = 1; i <= elements.Length; i++)
            elementSelector.Add(i.ToString(), elements[i - 1]);
        return elementSelector;
    }

    private void PaletteKey()
    {
        Console.WriteLine($"Primary color is {settings.Palette.Primary}");
        Console.WriteLine($"Background color is {settings.Palette.Background}");
        Console.WriteLine("Enter color number to select");
        Console.WriteLine("Or any other key to keep the current one");
        foreach (var (key, color) in colorSelector.OrderBy(pair => pair.Key))
            Console.WriteLine($"{key} - {color.Name}");
        Console.WriteLine("Enter primary color");
        settings.Palette.Primary = ReadColor(true);
        Console.WriteLine();
        Console.WriteLine("Enter background color");
        settings.Palette.Background = ReadColor(false);
        Console.WriteLine();
    }

    private Color ReadColor(bool isPrimary)
    {
        var input = Console.ReadLine() ?? "";
        var deafultColor = isPrimary
            ? settings.Palette.Primary
            : settings.Palette.Background;

        if (!colorSelector.ContainsKey(input))
            return deafultColor;
        return colorSelector[input];
    }

    private void FontKey()
    {
        if (!TryReadFontFamily(out var family)
            || !TryReadFontStyle(out var style))
            return;

        settings.Font = new Font(family, settings.Font.Size, style);
    }

    private bool TryReadFontFamily(out FontFamily family)
    {
        foreach (var (key, font) in fontFamilySelector.OrderBy(pair => pair.Key))
            Console.WriteLine($"{key} - {font.Name}");
        Console.WriteLine($"{fontFamilySelector.Count + 1} - Back");
        var answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        family = settings.Font.FontFamily;
        if (!fontFamilySelector.ContainsKey(answer))
            return false;

        family = fontFamilySelector[answer];
        return true;
    }

    private bool TryReadFontStyle(out FontStyle style)
    {
        foreach (var (key, fontStyle) in styleSelector.OrderBy(pair => pair.Key))
            Console.WriteLine($"{key} - {fontStyle}");
        Console.WriteLine($"{styleSelector.Count + 1} - Back");
        var answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        style = settings.Font.Style;
        if (!styleSelector.ContainsKey(answer))
            return false;

        style = styleSelector[answer];
        return true;
    }

    private void FormatKey()
    {
        foreach (var (key, format) in formatSelector.OrderBy(pair => pair.Key))
            Console.WriteLine($"{key} - {format}");
        Console.WriteLine($"{formatSelector.Count + 1} - Back");
        var answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        if (!formatSelector.ContainsKey(answer))
            return;

        settings.Format = formatSelector[answer];
    }

    private void SizeKey()
    {
        Console.WriteLine($"Image size is {settings.ImageSize}");
        Console.WriteLine("Enter new size as");
        Console.WriteLine("HEIGHT WIDTH");
        Console.WriteLine("Or pass an empty string to be brought back to menu");
        var answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        if (answer == "")
            return;

        var result = ParseSize(answer);

        if (result.IsSuccess)
            settings.ImageSize = result.Value;
        else
            Console.WriteLine(result.Error);
    }

    private Result<Size> ParseSize(string input)
    {
        var split = input.Split(' ');

        if (split.Length != 2)
            return Result.Fail<Size>("Enter two values separated by space");

        if (int.TryParse(split[0], out var height)
            && int.TryParse(split[1], out var width))
        {
            if (height <= 0 || width <= 0)
                Result.Fail<None>("Both values must be positive");
            return Result.Ok(new Size(height, width));
        }

        return Result.Fail<Size>("Values must be integer numbers");
    }
}