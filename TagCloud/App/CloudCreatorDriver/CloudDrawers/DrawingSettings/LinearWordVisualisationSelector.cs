using System.Drawing;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

public class LinearWordVisualisationSelector : IWordsVisualisationSelector
{
    private readonly List<Color> possibleColors = new();
    private readonly string wordsFont;
    private int maxSize;
    private double maxTf = 1;
    private int minSize;

    // ReSharper disable once RedundantDefaultMemberInitializer
    private double minTf = 0;


    public LinearWordVisualisationSelector(string wordsFont, int minSize, int maxSize)
    {
        this.wordsFont = wordsFont;
        SetWordsSizes(minSize, maxSize);
    }

    public Result<IDrawingWord> GetWordVisualisation(IWord word, Rectangle rectangle)
    {
        var fontDelta = maxSize - minSize;
        var tfDelta = maxTf - minTf;
        return Result.FailIf(possibleColors, possibleColors.Count == 0, "Possible colors are not initialised")
            .Then(_ => Result.FailIf(tfDelta, Math.Abs(tfDelta) < 1e-5, "TfDelta can not be 0"))
            .Then(_ => Result.Of(() =>
            {
                var size = minSize + (int)Math.Floor(fontDelta * (word.Tf - minTf) / tfDelta);
                var colorIdx = (int)Math.Floor(1d * (possibleColors.Count - 1) * (word.Tf - minTf) / tfDelta);
                return new DrawingWord(word, new Font(wordsFont, size), possibleColors[colorIdx], rectangle) as
                    IDrawingWord;
            }));
    }

    public Result<None> AddWordPossibleColors(IEnumerable<Color> colors)
    {
        return Result.OfAction(() => possibleColors.AddRange(colors));
    }

    public Result<None> SetWordsSizes(int min, int max)
    {
        if (min > max)
            return Result.Fail<None>($"Min font size {min} should be less or equal max size {max}");
        if (min <= 0)
            return Result.Fail<None>($"Min font value {min} should be positive");
        minSize = min;
        maxSize = max;
        return Result.Ok();
    }

    public Result<None> SetMinAndMaxRealWordTfIndex(double min, double max)
    {
        if (min > max)
            return Result.Fail<None>($"Min tf size {min} should be less or equal max size {max}");
        if (min <= 0)
            return Result.Fail<None>($"Min tf value {min} should be positive");
        minTf = min;
        maxTf = max;
        return Result.Ok();
    }

    public bool Empty()
    {
        return possibleColors.Count == 0;
    }
}