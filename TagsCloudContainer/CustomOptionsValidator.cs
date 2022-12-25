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
            return Result.Fail<ICustomOptions>("WorkingDirectory folder does not exist in root directory");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, "mystem.exe")))
            return Result.Fail<ICustomOptions>($"Mystem.exe not found in {options.WorkingDirectory} folder");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, options.WordsFileName)))
            return Result.Fail<ICustomOptions>($"{options.WorkingDirectory} does not contain file with words to draw");
        if (!File.Exists(Path.Combine(options.WorkingDirectory, options.BoringWordsName)))
            return Result.Fail<ICustomOptions>($"{options.WorkingDirectory} does not contain file with excluded words");
        var color = Color.FromName(options.BackgroundColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>("Unknown backgroud color");
        color = Color.FromName(options.FontColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>("Unknown font color");
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
            return Result.Fail<ICustomOptions>("Unsupported image format");
        return options.AsResult<ICustomOptions>();
    }
}