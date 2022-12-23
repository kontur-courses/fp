using System.Drawing;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainer;

public class CustomOptionsValidator
{
    private static readonly HashSet<string> acceptableFormats = new()
    {
        "bmp",
        "emf",
        "exif",
        "gif",
        "icon",
        "jpeg",
        "memorybmp",
        "tiff",
        "wmf",
        "png"
    };

    public static Result<ICustomOptions> ValidateOptions(CustomOptions options)
    {

        if (!Directory.Exists(options.WorkingDir))
            return Result.Fail<ICustomOptions>("Texts directory does not exist");
        if (!File.Exists(Path.Combine(options.WorkingDir, "mystem.exe")))
            return Result.Fail<ICustomOptions>("Mystem not found in working directory");
        if (!File.Exists(Path.Combine(options.WorkingDir, options.WordsFileName)))
            return Result.Fail<ICustomOptions>("Tag file does not exist");
        if (!File.Exists(Path.Combine(options.WorkingDir, options.BoringWordsName)))
            return Result.Fail<ICustomOptions>("Exclude words file does not exist");
        if (!File.Exists(Path.Combine(options.WorkingDir, "mystem.exe")))
            return Result.Fail<ICustomOptions>("Mystem not found in working directory");
        var color = Color.FromName(options.BackgroundColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>("Invalid backgroud color");
        color = Color.FromName(options.FontColor);
        if (!color.IsKnownColor)
            return Result.Fail<ICustomOptions>("Invalid font color");
        if (options.MinTagSize < 1)
            return Result.Fail<ICustomOptions>("Font size should be above 0");
        if (options.PictureSize < 1)
            return Result.Fail<ICustomOptions>("Picture size should be above 0");
        if (options.MaxTagSize >= options.PictureSize)
            return Result.Fail<ICustomOptions>("Font size should be less than picture size");
        var font = new Font(options.Font, 1);
        if (font.Name != options.Font)
            return Result.Fail<ICustomOptions>($"Font \"{options.Font}\" can't be found");
        if (!acceptableFormats.Contains(options.ImageFormat.ToLower()))
            return Result.Fail<ICustomOptions>("Unsupported image format");
        return options.AsResult<ICustomOptions>();
    }
}