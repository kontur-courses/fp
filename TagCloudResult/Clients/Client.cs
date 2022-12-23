using System.Drawing;
using ResultOfTask;
using TagCloudResult.Curves;
using TagCloudResult.Extensions;
using TagCloudResult.Savers;
using TagCloudResult.Words;

namespace TagCloudResult.Clients;

public class Client
{
    private readonly CloudDrawer _drawer;
    private readonly CloudLayouter _layouter;
    private readonly IBitmapSaver _saver;

    public Client(CloudLayouter layouter, CloudDrawer drawer, IBitmapSaver saver)
    {
        _layouter = layouter;
        _drawer = drawer;
        _saver = saver;
    }

    public Bitmap Draw(IEnumerable<Word> words, ICurve curve, Size size, Font font, IEnumerable<Color> colors)
    {
        var wordRectangles = new List<WordRectangle>();
        var fontSize = font.Size;
        foreach (var word in words.OrderByDescending(word => word.Frequency))
        {
            var wordRectangle = Result.Of(() => font.ChangeSize(fontSize * word.Frequency))
                .Then(word.MeasureWord)
                .Then(rectangleSize => _layouter.PutRectangle(curve, rectangleSize))
                .Then(rectangle => new WordRectangle(word) { Rectangle = rectangle })
                .OnFail(PrintError);
            wordRectangles.Add(wordRectangle.Value);
        }

        font = font.ChangeSize(fontSize);
        var image = _drawer.CreateImage(wordRectangles, size, font, colors).OnFail(PrintError);
        return image.Value;
    }

    public void Save(Bitmap image, IEnumerable<string> destinationPaths)
    {
        foreach (var destinationPath in destinationPaths) Save(image, destinationPath).OnFail(PrintError);
    }

    public Result<None> Save(Bitmap image, string destinationPath)
    {
        return Helper.GetImageFormat(destinationPath)
            .Then(format => _saver.Save(image, destinationPath, format));
    }

    public void PrintError(string error)
    {
        Console.WriteLine(error);
        Environment.Exit(1);
    }
}