using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using TagsCloud.Contracts;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloud.Results;

namespace TagsCloud.Builders;

public class OutputOptionsBuilder
{
    private Color backgroundColor;
    private IImageEncoder imageEncoder;
    private Size imageSize;

    public Result<OutputOptionsBuilder> SetImageFormat(ImageFormat format)
    {
        imageEncoder = format switch
        {
            ImageFormat.Jpeg or ImageFormat.Jpg => new JpegEncoder(),
            ImageFormat.Bmp => new BmpEncoder(),
            _ => new PngEncoder()
        };

        return this;
    }

    public Result<OutputOptionsBuilder> SetImageSize(int width, int height)
    {
        if (width <= 0 || height <= 0)
            return ResultExtensions.Fail<OutputOptionsBuilder>(
                $"Width and height can't be <= 0! Width = {width}, Height = {height}");

        imageSize = new Size(width, height);
        return this;
    }

    public Result<OutputOptionsBuilder> SetImageBackgroundColor(string hex)
    {
        backgroundColor = Color.TryParseHex(hex, out var color) ? color : Color.White;
        return this;
    }

    public Result<IOutputProcessorOptions> BuildOptions()
    {
        return new OutputProcessorOptions
        {
            BackgroundColor = backgroundColor,
            ImageEncoder = imageEncoder,
            ImageSize = imageSize
        };
    }
}