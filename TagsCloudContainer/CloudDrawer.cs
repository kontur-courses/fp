using System.Drawing;
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
    private readonly IImageFormatProvider formatProvider;

    public CloudDrawer(ISpiralDrawer spiralDrawer, IConverter converter,
        IWordSizeCalculator calculator, IImageFormatProvider formatProvider)
    {
        this.spiralDrawer = spiralDrawer;
        this.converter = converter;
        this.calculator = calculator;
        this.formatProvider = formatProvider;
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
        var dictionaryResult = converter.GetWordsInFile(options);
        if (!dictionaryResult.IsSuccess)
            return Result.Fail<string>(dictionaryResult.Error);
        var wordsToDrawResult = calculator.CalculateSize(dictionaryResult.GetValueOrThrow(), options);
        if (!wordsToDrawResult.IsSuccess)
            return Result.Fail<string>(wordsToDrawResult.Error);
        foreach (var pair in wordsToDrawResult.GetValueOrThrow())
        {
            var stringSize = graphics.MeasureString(pair.Key, pair.Value);
            var putResult = layout.PutNextRectangle(stringSize);
            if (!putResult.IsSuccess)
                return Result.Fail<string>($"Word {pair.Key} can't be placed in layout");
            graphics.DrawString(pair.Key, pair.Value, fontColor, putResult.GetValueOrThrow());
        }

        var format = formatProvider.GetFormat(options.ImageFormat);
        if (!format.IsSuccess)
            return Result.Fail<string>(dictionaryResult.Error);
        picture.Save(path, format.GetValueOrThrow());
        return $"Picture saved at {path}".AsResult();
    }

    public Result<string> DrawCloud(ICustomOptions options)
    {
        return DrawCloud(
            Path.Combine(options.WorkingDirectory,
                string.Concat(options.ImageName, ".", options.ImageFormat.ToLower())), options);
    }
}