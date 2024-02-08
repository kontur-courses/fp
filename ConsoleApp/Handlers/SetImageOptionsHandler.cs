using System.Globalization;
using ConsoleApp.Options;
using SixLabors.ImageSharp;
using TagsCloudContainer;
using TagsCloudContainer.Settings;

namespace ConsoleApp.Handlers;

public class SetImageOptionsHandler : IOptionsHandler
{
    private readonly IImageSettings imageSettings;

    public SetImageOptionsHandler(IImageSettings imageSettings)
    {
        this.imageSettings = imageSettings;
    }

    public bool CanParse(IOptions options)
    {
        return options is SetImageOptions;
    }

    public Result<string> WithParsed(IOptions options)
    {
        if (options is not SetImageOptions opts)
            return Result.Fail<string>("Не удалось определить настройки изображения.");

        Map(opts);
        return "Настройки изображения установлены.";
    }

    private void Map(SetImageOptions options)
    {
        FromHex(options.PrimaryColor)
            .Then(color => imageSettings.PrimaryColor = color);
        FromHex(options.BackgroundColor)
            .Then(color => imageSettings.BackgroundColor = color);
        if (options.Width != default)
            imageSettings.ImageSize = new Size(options.Width, imageSettings.ImageSize.Height);
        if (options.Height != default)
            imageSettings.ImageSize = new Size(imageSettings.ImageSize.Width, options.Height);
        if (options.Font is not null)
            imageSettings.TextOptions.Font = options.Font;
    }

    private static Result<Color> FromHex(string hexColor)
    {
        if (hexColor.Length == 6 && int.TryParse(hexColor, NumberStyles.HexNumber, null, out var colorValue))
        {
            return Color.FromRgb((byte)((colorValue & 0xFF0000) >> 16), (byte)((colorValue & 0x00FF00) >> 8),
                (byte)(colorValue & 0x0000FF));
        }

        return Result.Fail<Color>("Неверный формат.");
    }
}