using System.Drawing.Imaging;
using System.Drawing;

namespace TagsCloudVisualization;

public class LayoutDrawer
{
    private IInterestingWordsParser interestingWordsParser;
    private IRectangleLayouter rectangleLayouter;
    private IPalette palette;
    private Font font;

    public LayoutDrawer(IInterestingWordsParser interestingWordsParser,
        IRectangleLayouter rectangleLayouter,
        IPalette palette,
        Font font)
    {
        this.interestingWordsParser = interestingWordsParser;
        this.rectangleLayouter = rectangleLayouter;
        this.palette = palette;
        this.font = font;
        if (string.Compare(font.OriginalFontName, font.Name, StringComparison.InvariantCultureIgnoreCase) != 0)
            Console.WriteLine($"Font \"{font.OriginalFontName}\" was not found. Using \"{font.Name}\" instead");
    }

    public Result<Bitmap> CreateLayoutImageFromFile(string inputFilePath,
        Size imageSize,
        int minimumFontSize)
    {
        return interestingWordsParser
            .GetInterestingWords(inputFilePath)
            .Then(GetSortedInterestingWordsCount)
            .Then(rectangles => DrawRectangles(imageSize, minimumFontSize, rectangles))
            .RefineError("Can't create layout image");
    }

    private Bitmap DrawRectangles(Size imageSize,
        int minimumFontSize,
        IOrderedEnumerable<(string Word, int Count)> sortedWordsCount)
    {
        var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        var mostWordOccurrencies = sortedWordsCount.Max(arg => arg.Count);

        graphics.Clear(palette.GetBackgroundColor());

        foreach (var wordCount in sortedWordsCount)
        {
            var rectangleFont = new Font(font.FontFamily,
                Math.Max(font.Size * wordCount.Count / mostWordOccurrencies, minimumFontSize));
            var rectangleSize = graphics.MeasureString(wordCount.Word, rectangleFont);

            var textRectangle = rectangleLayouter.PutNextRectangle(rectangleSize);
            var x = textRectangle.X + imageSize.Width / 2;
            var y = textRectangle.Y + imageSize.Height / 2;

            if (x < 0 || x > imageSize.Width || y < 0 || y > imageSize.Height)
                throw new ApplicationException("Words went out of image boundaries");

            using var brush = new SolidBrush(palette.GetNextWordColor());
            graphics.DrawString(wordCount.Word, rectangleFont, brush, x, y);
        }

        return bitmap;
    }

    private IOrderedEnumerable<(string Word, int Count)> GetSortedInterestingWordsCount(
        IEnumerable<string> interestingWords)
    {
        var sortedWordsCount = interestingWords
            .GroupBy(s => s)
            .Select(group => (Word: group.Key, Count: group.Count()))
            .OrderByDescending(wordCount => wordCount.Count);

        return sortedWordsCount;
    }
}