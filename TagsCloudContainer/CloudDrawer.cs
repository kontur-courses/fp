using System.Drawing;
using System.Drawing.Imaging;
using CloudLayout;
using CloudLayout.Interfaces;
using TagsCloudContainer.Interfaces;
using Result;

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

    public string DrawCloud(string path, ICustomOptions options)
    {
        var layout = new CircularCloudLayout(spiralDrawer, new InputOptions(options.PictureSize));
        var size = options.PictureSize;
        using var picture = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(picture);
        var backColor = Color.FromName(options.BackgroundColor);
        using var fontColor = new SolidBrush(Color.FromName(options.FontColor));
        graphics.Clear(backColor);
        var resultDictionary = converter.GetWordsInFile(options);
        if (!resultDictionary)
            return resultDictionary.Exception!.Message;
        var wordsToDrawResult = calculator.CalculateSize(resultDictionary.Value, options);
        if (!wordsToDrawResult)
            return resultDictionary.Exception!.Message;
        foreach (var pair in wordsToDrawResult.Value)
        {
            var stringSize = graphics.MeasureString(pair.Key, pair.Value);
            var putResult = layout.PutNextRectangle(stringSize);
            if (!putResult)
                return resultDictionary.Exception!.Message;
            if (!putResult.Value.isPutted)
                return $"Word {pair.Key} can't be placed in layout";
            graphics.DrawString(pair.Key, pair.Value, fontColor, putResult.Value.rectangle);
        }

        var format = GetImageFormat(options.ImageFormat);
        if (!format)
            return resultDictionary.Exception!.Message;
        picture.Save(path, format.Value);
        return $"Picture saved at {path}";
    }

    public string DrawCloud(ICustomOptions options)
    {
        return DrawCloud(
            Path.Combine(options.WorkingDir,
                string.Concat(options.ImageName, ".", options.ImageFormat.ToLower())), options);
    }

    private static Result<ImageFormat> GetImageFormat(string format)
    {
        return format.ToLower() switch
        {
            "png" => new Result<ImageFormat>(ImageFormat.Png),
            "bmp" => new Result<ImageFormat>(ImageFormat.Bmp),
            "emf" => new Result<ImageFormat>(ImageFormat.Emf),
            "exif" => new Result<ImageFormat>(ImageFormat.Exif),
            "gif" => new Result<ImageFormat>(ImageFormat.Gif),
            "icon" => new Result<ImageFormat>(ImageFormat.Icon),
            "jpeg" => new Result<ImageFormat>(ImageFormat.Jpeg),
            "memorybmp" => new Result<ImageFormat>(ImageFormat.MemoryBmp),
            "tiff" => new Result<ImageFormat>(ImageFormat.Tiff),
            "wmf" => new Result<ImageFormat>(ImageFormat.Wmf),
            _ => new Result<ImageFormat>(new ArgumentException("Unexpected image format"))
        };
    }
}