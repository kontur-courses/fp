using System.Drawing;
using System.Drawing.Drawing2D;
using ResultOfTask;
using TagCloudResult.Extensions;
using TagCloudResult.Words;

namespace TagCloudResult;

public class CloudDrawer
{
    private readonly IList<Color> _defaultColors = new List<Color> { Color.Black };
    private readonly Random _random;

    public CloudDrawer(Random random)
    {
        _random = random;
    }

    public Result<Bitmap> CreateImage(IList<WordRectangle> words, Size size, Font font, IEnumerable<Color> colors)
    {
        colors = colors.Any() ? colors : _defaultColors;
        return Result.Of(() => CreateBitmap(size))
            .Then(image => DrawWords(image, words, font, colors))
            .RefineError("Couldn't create image.");
    }

    private Bitmap CreateBitmap(Size size)
    {
        return new Bitmap(size.Width, size.Height);
    }

    private Bitmap DrawWords(Bitmap image, IList<WordRectangle> wordsRectangles, Font font, IEnumerable<Color> colors)
    {
        using var graphics = Graphics.FromImage(image);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        var fontSize = font.Size;

        foreach (var wordRectangle in wordsRectangles)
        {
            var color = GetRandomColorFromColors(colors);
            using var brush = new SolidBrush(color);
            font = font.ChangeSize(fontSize * wordRectangle.Word.Frequency);
            graphics.DrawString(wordRectangle.Word.Value, font, brush,
                wordRectangle.Rectangle.Location + image.Size.Multiply(0.5));
        }

        return image;
    }

    private Color GetRandomColorFromColors(IEnumerable<Color> colors)
    {
        var colorsList = colors.ToList();
        var randomNumber = _random.Next(colorsList.Count);
        return colorsList[randomNumber];
    }
}