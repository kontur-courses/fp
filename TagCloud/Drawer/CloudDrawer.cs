using System.Drawing;
using System.Drawing.Text;
using ResultOf;
using TagCloud.AppSettings;
using TagCloud.Layouter;
using TagCloud.PointGenerator;

namespace TagCloud.Drawer;

public class CloudDrawer : IDrawer
{
    private readonly IPointGeneratorProvider pointGeneratorProvider;
    private readonly IPaletteProvider paletteProvider;
    private readonly IAppSettings appSettings;
    private int minimalRank;
    private int maximalRank;
    private const int MaximalFontSize = 50;
    private const int LengthSizeMultiplier = 35;

    public CloudDrawer(IPointGeneratorProvider pointGeneratorProvider, IPaletteProvider paletteProvider,
        IAppSettings appSettings)
    {
        this.pointGeneratorProvider = pointGeneratorProvider;
        this.paletteProvider = paletteProvider;
        this.appSettings = appSettings;
    }

    public Result<Bitmap> DrawTagCloud(IEnumerable<(string word, int rank)> words)
    {
        var pointGenerator = pointGeneratorProvider.CreateGenerator(appSettings.LayouterType);

        if (!pointGenerator.IsSuccess)
            return Result.Fail<Bitmap>(pointGenerator.Error);

        var palette = paletteProvider.CreatePalette(appSettings.ImagePalette);

        if (!palette.IsSuccess)
            return Result.Fail<Bitmap>(palette.Error);

        var layouter = new CloudLayouter(pointGenerator.Value);
        var tags = PlaceWords(words, layouter);

        return !ValidateImageBorders(tags)
            ? Result.Fail<Bitmap>(
                $"Tags don't fit to given image size of {appSettings.CloudWidth}x{appSettings.CloudHeight}")
            : Result.Ok(DrawTags(tags, palette.Value));
    }

    private Bitmap DrawTags(IEnumerable<Tag> tags, IPalette palette)
    {
        var imageSize = new Size(appSettings.CloudWidth, appSettings.CloudHeight);
        var image = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(image);
        using var background = new SolidBrush(palette.BackgroundColor);
        graphics.FillRectangle(background, 0, 0, imageSize.Width, imageSize.Height);
        foreach (var tag in tags)
        {
            var pointFCoordinates = new PointF(tag.Position.X, tag.Position.Y);
            using var brush = new SolidBrush(palette.ForegroundColor);
            graphics.DrawString(tag.Value, new Font(appSettings.FontType, tag.FontSize), brush, pointFCoordinates);
        }

        return image;
    }

    private IEnumerable<Tag> PlaceWords(IEnumerable<(string word, int rank)> words, ILayouter layouter)
    {
        maximalRank = words.First().rank;
        minimalRank = words.Last().rank - 1;

        var tags = new List<Tag>();

        foreach (var pair in words)
        {
            var fontSize = CalculateFontSize(pair.rank);
            var boxLength = CalculateWordBoxLength(pair.word.Length, fontSize);
            var rectangle = layouter.PutNextRectangle(new Size(boxLength, fontSize));
            tags.Add(new Tag(pair.word, rectangle, fontSize));
        }

        return tags;
    }

    private int CalculateFontSize(int rank)
    {
        return (MaximalFontSize * (rank - minimalRank)) / (maximalRank - minimalRank);
    }

    private int CalculateWordBoxLength(int length, int fontSize)
    {
        return (int)Math.Round(length * LengthSizeMultiplier * ((double)fontSize / MaximalFontSize));
    }

    private bool ValidateImageBorders(IEnumerable<Tag> tags)
    {
        var tagsPositions = tags.Select(t => t.Position);
        var minX = tagsPositions.Min(p => p.Left);
        var maxX = tagsPositions.Max(p => p.Right);
        var minY = tagsPositions.Min(p => p.Top);
        var maxY = tagsPositions.Max(p => p.Bottom);

        var width = maxX - minX;
        var height = maxY - minY;

        return width <= appSettings.CloudWidth && height <= appSettings.CloudHeight;
    }
}