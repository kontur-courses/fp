using System.Drawing;
using TagsCloudContainer.Interfaces;
using Result;

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
            return new Result<ICustomOptions>(new ArgumentException("Texts directory does not exist"));
        if (!File.Exists(Path.Combine(options.WorkingDir, "mystem.exe")))
            return new Result<ICustomOptions>(new ArgumentException("Mystem not found in working directory"));
        if (!File.Exists(Path.Combine(options.WorkingDir, options.WordsFileName)))
            return new Result<ICustomOptions>(new ArgumentException("Tag file does not exist"));
        if (!File.Exists(Path.Combine(options.WorkingDir, options.BoringWordsName)))
            return new Result<ICustomOptions>(new ArgumentException("Exclude words file does not exist"));
        if (!File.Exists(Path.Combine(options.WorkingDir, "mystem.exe")))
            return new Result<ICustomOptions>(new ArgumentException("Mystem not found in working directory"));
        var color = Color.FromName(options.BackgroundColor);
        if (!color.IsKnownColor)
            return new Result<ICustomOptions>(new ArgumentException("Invalid backgroud color"));
        color = Color.FromName(options.FontColor);
        if (!color.IsKnownColor)
            return new Result<ICustomOptions>(new ArgumentException("Invalid font color"));
        if (options.MinTagSize < 1)
            return new Result<ICustomOptions>(new ArgumentException("Font size should be above 0"));
        if (options.PictureSize < 1)
            return new Result<ICustomOptions>(new ArgumentException("Picture size should be above 0"));
        if (options.MaxTagSize >= options.PictureSize)
            return new Result<ICustomOptions>(new ArgumentException("Font size should be less than picture size"));
        var font = new Font(options.Font, 1);
        if (font.Name != options.Font)
            return new Result<ICustomOptions>(new ArgumentException($"Font \"{options.Font}\" can't be found"));
        if (!acceptableFormats.Contains(options.ImageFormat.ToLower()))
            return new Result<ICustomOptions>(new ArgumentException("Unsupported image format"));
        return new Result<ICustomOptions>(options);
    }
}