using System.Drawing;
using System.Drawing.Imaging;
using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.Saver;

public class ImageSaver : IImageSaver
{
    public static readonly IReadOnlyDictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>
    {
        ["png"] = ImageFormat.Png,
        ["bmp"] = ImageFormat.Bmp,
        ["jpg"] = ImageFormat.Jpeg,
        ["tiff"] = ImageFormat.Tiff,
        ["gif"] = ImageFormat.Gif,
        ["exif"] = ImageFormat.Exif
    };

    public Result<None> Save(Bitmap bitmap, string outputPath, string outputFormat)
    {
        if (!ImageFormats.TryGetValue(outputFormat.ToLowerInvariant(), out var imageFormat))
            return Result.Fail<None>($"Acceptable formats: {string.Join(',', ImageFormats.Keys)}, but was {outputFormat.ToLowerInvariant()}");

        try
        {
            bitmap.Save($"{outputPath}.{outputFormat.ToLowerInvariant()}", imageFormat);
        }
        catch (Exception e)
        {
            return Result.Fail<None>(e.Message);
        }

        return Result.Ok();
    }
}