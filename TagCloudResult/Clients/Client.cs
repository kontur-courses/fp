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

    public Font Font { get; set; }
    public ICurve Curve { get; set; }
    public Size ImageSize { get; set; }

    public Result<Bitmap> Draw(IEnumerable<Word> words, IEnumerable<Color> colors)
    {
        var wordRectangles = new List<WordRectangle>();
        var fontSize = Font.Size;
        using var wordMeasurer = new WordMeasurer();
        foreach (var word in words)
            Result.Of(() => Font.ChangeSize(fontSize * word.Frequency))
                .Then(font => wordMeasurer.MeasureWord(word, font))
                .Then(rectangleSize => _layouter.PutRectangle(Curve, rectangleSize))
                .Then(rectangle => new WordRectangle(word) { Rectangle = rectangle })
                .Then(wordRectangles.Add);

        Font = Font.ChangeSize(fontSize);
        var image = _drawer.CreateImage(wordRectangles, ImageSize, Font, colors);
        return image;
    }

    public Result<None> Save(Bitmap image, IEnumerable<string> destinationPaths)
    {
        foreach (var destinationPath in destinationPaths)
        {
            var result = Save(image, destinationPath);
            if (result.IsSuccess == false)
                return result;
        }

        return Result.Ok();
    }

    public Result<None> Save(Bitmap image, string destinationPath)
    {
        return Helper.GetImageFormat(destinationPath)
            .Then(format => _saver.Save(image, destinationPath, format));
    }
}