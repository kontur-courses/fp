using System.Drawing;
using System.Drawing.Imaging;
using CloudLayout;
using CloudLayout.Interfaces;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainer;

public class CloudDrawer : IDrawer
{
    private readonly ISpiralDrawer spiralDrawer;
    private readonly IConverter converter;
    private readonly IWordSizeCalculator calculator;

    public CloudDrawer(ISpiralDrawer spiralDrawer, IConverter converter,
        IWordSizeCalculator calculator)
    {
        this.spiralDrawer = spiralDrawer;
        this.converter = converter;
        this.calculator = calculator;
    }

    public Result<string> DrawCloud(string path, ICustomOptions options)
    {
        var layout = new CircularCloudLayout(spiralDrawer, new InputOptions(options.PictureSize));
        var size = options.PictureSize;
        using var picture = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(picture);
        var backColor = Color.FromName(options.BackgroundColor);
        using var fontColor = new SolidBrush(Color.FromName(options.FontColor));
        graphics.Clear(backColor);
        var resultDictionary = converter.GetWordsInFile(options);
        if (!resultDictionary.IsSuccess)
            return resultDictionary.Error.AsResult();
        var wordsToDrawResult = calculator.CalculateSize(resultDictionary.GetValueOrThrow(), options);
        if (!wordsToDrawResult.IsSuccess)
            return resultDictionary.Error.AsResult();
        foreach (var pair in wordsToDrawResult.GetValueOrThrow())
        {
            var stringSize = graphics.MeasureString(pair.Key, pair.Value);
            var putResult = layout.PutNextRectangle(stringSize);
            if (!putResult.IsSuccess)
                return Result.Fail<string>(putResult.Error);
            if (!putResult.GetValueOrThrow().isPutted)
                return $"Word {pair.Key} can't be placed in layout".AsResult();
            graphics.DrawString(pair.Key, pair.Value, fontColor, putResult.GetValueOrThrow().rectangle);
        }

        var format = GetImageFormat(options.ImageFormat);
        if (!format.IsSuccess)
            return resultDictionary.Error.AsResult();
        picture.Save(path, format.GetValueOrThrow());
        return $"Picture saved at {path}".AsResult();
    }

    public Result<string> DrawCloud(ICustomOptions options)
    {
        return DrawCloud(
            Path.Combine(options.WorkingDir,
                string.Concat(options.ImageName, ".", options.ImageFormat.ToLower())), options);
    }

    private static Result<ImageFormat> GetImageFormat(string format)
    {
        return format.ToLower() switch
        {
            "png" => ImageFormat.Png.AsResult(),
            "bmp" => ImageFormat.Bmp.AsResult(),
            "emf" => ImageFormat.Emf.AsResult(),
            "exif" => ImageFormat.Exif.AsResult(),
            "gif" => ImageFormat.Gif.AsResult(),
            "icon" => ImageFormat.Icon.AsResult(),
            "jpeg" => ImageFormat.Jpeg.AsResult(),
            "memorybmp" => ImageFormat.MemoryBmp.AsResult(),
            "tiff" => ImageFormat.Tiff.AsResult(),
            "wmf" => ImageFormat.Wmf.AsResult(),
            _ => Result.Fail<ImageFormat>("Unexpected image format")
        };
    }
}