using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Words;

namespace TagsCloudVisualization;

public class TagCloudVisualizations
{
    private readonly ICloudLayouter _cloudLayouter;
    private readonly IWordsLoader _wordsLoader;


    public TagCloudVisualizations(ICloudLayouter layouter, IWordsLoader wordsLoader)
    {
        _cloudLayouter = layouter;
        _wordsLoader = wordsLoader;
    }


    public Result<Bitmap> DrawCloud(VisualizationOptions options)
    {
        var checkResult = CheckOptions(options);
        if (!checkResult.IsSuccess)
            return Result.Fail<Bitmap>(checkResult.Error);

        _cloudLayouter.Reset();
        var center = new Point(options!.CanvasSize!.Value.Width / 2, options.CanvasSize.Value.Height / 2);
        var layoutOptions = new LayoutOptions(center, options.SpiralStep);

        var size = options.CanvasSize;
        var bitmap = new Bitmap(size.Value.Width, size.Value.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(options.BackgroundColor!.Value);

        var wordsToDraw = _wordsLoader.LoadWords(options);
        if (!wordsToDraw.IsSuccess)
            return Result.Fail<Bitmap>(wordsToDraw.Error);

        var wordIndex = 0;
        foreach (var word in wordsToDraw.Value)
        {
            var fontForWord = new Font(options.FontFamily!, word.Size);
            var stringSize = graphics.MeasureString(word.Text, fontForWord);
            var rectangle = _cloudLayouter.PutNextRectangle(stringSize, layoutOptions);
            if (!rectangle.IsSuccess)
                continue;

            if (rectangle.Value.Left < 0 || rectangle.Value.Top < 0 || rectangle.Value.Right > options.CanvasSize.Value.Width || rectangle.Value.Bottom > options.CanvasSize.Value.Height)
                return Result.Fail<Bitmap>($"Word {word.Text} out of canvas bounds");

            var fontColor = GetNextFontColor(options, wordIndex);
            graphics.DrawString(word.Text, fontForWord, fontColor, rectangle.Value);
            wordIndex++;
        }

        return bitmap;
    }

    private static Brush GetNextFontColor(VisualizationOptions options, int wordIndex)
    {
        if (!options.Palette!.AvailableBrushes!.Any())
            return options.Palette!.DefaultBrush!;

        if (options.Palette.AvailableBrushes!.Count == 1)
            return options.Palette.AvailableBrushes[0];

        var brushesCount = options.Palette.AvailableBrushes.Count;
        var brushIndex = wordIndex % brushesCount;
        return options.Palette.AvailableBrushes[brushIndex];
    }


    private static Result<None?> CheckOptions(VisualizationOptions options)
    {
        if (options.CanvasSize == null)
            return Result.Fail<None?>("Canvas null");

        if (options.CanvasSize.Value.Width < 1 || options.CanvasSize.Value.Height < 1)
            return Result.Fail<None?>("Canvas size must be greater than 1");

        if (options.FontFamily == null)
            return Result.Fail<None?>("FontFamily null");

        if (options.Palette == null)
            return Result.Fail<None?>("Palette null");

        if (options.Palette.DefaultBrush == null)
            return Result.Fail<None?>("DefaultBrush null");

        if (options.Palette.AvailableBrushes == null)
            return Result.Fail<None?>("AvailableBrushes null");

        if (options.SpiralStep - 0 < 0.0001f)
            return Result.Fail<None?>("SpiralStep must be greater than 0");

        return Result.Ok();
    }
}