using System.Drawing;
using TagsCloudContainer.Cloud;

namespace TagsCloudContainer.WordProcessing;

public class WordRectangleGenerator
{
    private Settings _settings;
    private CircularCloudLayouter _layouter;
    private FontProvider _fontProvider;

    public WordRectangleGenerator(CircularCloudLayouter layouter, Settings settings, FontProvider fontProvider)
    {
        _layouter = layouter;
        _settings = settings;
        _fontProvider = fontProvider;
    }

    public Result<List<Rectangle>> GenerateRectangles(List<Word> words)
    {
        var rectangles = new List<Rectangle>();
        var settingsFont = _fontProvider.TryGetFont(_settings.FontName, _settings.FontSize);
        if (!settingsFont.IsSuccess)
            return Result.Fail<List<Rectangle>>(settingsFont.Error);
        foreach (var word in words)
        {
            using var font = new Font(settingsFont.Value.FontFamily, word.Size);
            var size = EditingWordSize(word.Value, font);
            var rectangle = _layouter.PutNextRectangle(size);
            if (!rectangle.IsSuccess)
                return Result.Fail<List<Rectangle>>(rectangle.Error);
            rectangles.Add(rectangle.Value);
        }

        return rectangles.Ok();
    }

    private static Size EditingWordSize(string word, Font font)
    {
        var bitmap = new Bitmap(1, 1);
        var graphics = Graphics.FromImage(bitmap);
        var result = graphics.MeasureString(word, font).ToSize();
        if (result.Width == 0) result.Width = 1;
        if (result.Height == 0) result.Height = 1;
        return result;
    }
}