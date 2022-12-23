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

    public Result<Bitmap> Draw(IEnumerable<Word> words, ICurve curve, Size size, Font font, IEnumerable<Color> colors)
    {
        var wordRectangles = new List<WordRectangle>();
        var fontSize = font.Size;
        foreach (var word in words.OrderByDescending(word => word.Frequency))
        {
            font = font.ChangeSize(fontSize * word.Frequency);
            var rectangleSize = word.MeasureWord(font);
            var rectangle = _layouter.PutRectangle(curve, rectangleSize);
            if (rectangle.IsSuccess == false)
                return Result.Fail<Bitmap>(rectangle.Error).RefineError("Couldn't draw image.");
            var wordRectangle = new WordRectangle(word) { Rectangle = rectangle.Value };
            wordRectangles.Add(wordRectangle);
        }

        font = font.ChangeSize(fontSize);
        var image = _drawer.CreateImage(wordRectangles, size, font, colors);
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

        return new Result<None>();
    }

    public Result<None> Save(Bitmap image, string destinationPath)
    {
        var format = Helper.GetImageFormat(destinationPath);
        if (format.IsSuccess == false)
            return Result.Fail<None>("Cannot define image extension.");
        var result = _saver.Save(image, destinationPath, format.Value);
        return result;
    }

    public void PrintError(string error)
    {
        Console.WriteLine(error);
        Environment.Exit(1);
    }
}