using System.Drawing;
using System.Drawing.Imaging;
using CloudLayout;
using CloudLayout.Interfaces;
using TagsCloudContainer.Interfaces;
using Result;

namespace TagsCloudContainer;

public class CloudDrawer
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

    public Result<string> DrawCloud(string path, Result<ICustomOptions> options)
    {
        var layout = new CircularCloudLayout(spiralDrawer, new InputOptions(options.Value.PictureSize));
        var size = options.Value.PictureSize;
        var picture = new Bitmap(size, size);
        var graphics = Graphics.FromImage(picture);
        var backColor = Color.FromName(options.Value.BackgroundColor);
        var fontColor = new SolidBrush(Color.FromName(options.Value.FontColor));
        graphics.Clear(backColor);
        var resultDictionary = converter.GetWordsInFile(options);
        if (!resultDictionary)
            return new Result<string>(resultDictionary.Exception!);
        var wordsToDraw = calculator.CalculateSize(resultDictionary, options);
        if (!wordsToDraw)
            return new Result<string>(wordsToDraw.Exception!);
        foreach (var pair in wordsToDraw.Value)
        {
            var stringSize = graphics.MeasureString(pair.Key, pair.Value);
            var putResult = layout.PutNextRectangle(stringSize);
            if (!putResult)
                return new Result<string>(putResult.Exception!);
            if (!putResult.Value.isPutted)
                return new Result<string>(new Exception($"Word {pair.Key} can't be placed in layout"));
            graphics.DrawString(pair.Key, pair.Value, fontColor, putResult.Value.rectangle);
        }

        var format = GetImageFormat(options.Value.ImageFormat);
        if (!format)
            return new Result<string>(format.Exception!);
        picture.Save(path, format.Value);
        return new Result<string>($"Picture saved at {path}");
    }

    public Result<string> DrawCloud(Result<ICustomOptions> options)
    {
        return DrawCloud(
            Path.Combine(options.Value.WorkingDir,
                string.Concat(options.Value.ImageName, ".", options.Value.ImageFormat.ToLower())), options);
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