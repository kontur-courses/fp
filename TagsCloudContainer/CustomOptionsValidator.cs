using System.Drawing;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainer;

public class CustomOptionsValidator : IOptionsValidator
{
    private readonly IImageFormatProvider formatProvider;

    public CustomOptionsValidator(IImageFormatProvider formatProvider)
    {
        this.formatProvider = formatProvider;
    }

    public Result<ICustomOptions> ValidateOptions(CustomOptions options)
    {
        if (!Directory.Exists(options.WorkingDirectory))
            return Result.Fail<ICustomOptions>($"{options.WorkingDirectory} folder should exist.");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, "mystem.exe")))
            return Result.Fail<ICustomOptions>(
                $"Mystem.exe should be in {options.WorkingDirectory} folder. You can download it from https://yandex.ru/dev/mystem/.");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, options.WordsFileName)))
            return Result.Fail<ICustomOptions>(
                $"{options.WorkingDirectory} should contain your file with words to draw.");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, options.BoringWordsName)))
            return Result.Fail<ICustomOptions>(
                $"{options.WorkingDirectory} should contain your text (*.txt) file with excluded words.");
        var color = Color.FromName(options.BackgroundColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>(
                $"Unknown background color.\r\nSupported colors listed here" +
                $" https://learn.microsoft.com/en-us/dotnet/api/system.drawing.color?view=net-7.0#properties");
        color = Color.FromName(options.FontColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>(
                "Unknown font color.\r\nSupported colors listed here" +
                " https://learn.microsoft.com/en-us/dotnet/api/system.drawing.color?view=net-7.0#properties");
        if (options.MinTagSize < 1)
            return Result.Fail<ICustomOptions>("Font size should be above 0");
        if (options.PictureSize < 1)
            return Result.Fail<ICustomOptions>("Picture size should be above 0");
        if (options.MaxTagSize >= options.PictureSize)
            return Result.Fail<ICustomOptions>("Font size should be less than picture size");
        var font = new Font(options.Font, 1);
        if (font.Name != options.Font)
            return Result.Fail<ICustomOptions>($"Font \"{options.Font}\" can't be found");
        if (!formatProvider.ValidateFormatName(options.ImageFormat))
            return Result.Fail<ICustomOptions>($"Unsupported image format. Supported formats are:" +
                                               $" {formatProvider.GetSupportedFormats()}");
        return options.AsResult<ICustomOptions>();
    }
}