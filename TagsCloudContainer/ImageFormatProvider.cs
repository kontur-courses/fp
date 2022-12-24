using System.Drawing.Imaging;
using ResultOf;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class ImageFormatProvider : IImageFormatProvider
{
    private readonly Dictionary<string, ImageFormat> formats = new()
    {
        { "png", ImageFormat.Png },
        { "bmp", ImageFormat.Bmp },
        { "emf", ImageFormat.Emf },
        { "exif", ImageFormat.Exif },
        { "gif", ImageFormat.Gif },
        { "icon", ImageFormat.Icon },
        { "jpeg", ImageFormat.Jpeg },
        { "memorybmp", ImageFormat.MemoryBmp },
        { "tiff", ImageFormat.Tiff },
        { "wmf", ImageFormat.Wmf }
    };

    public bool ValidateFormatName(string name) => formats.ContainsKey(name.ToLower());

    public Result<ImageFormat> GetFormat(string name)
    {
        name = name.ToLower();
        return !ValidateFormatName(name)
            ? Result.Fail<ImageFormat>("Image format unexpected conversion")
            : formats[name].AsResult();
    }
}