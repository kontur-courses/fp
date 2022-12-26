using System.Drawing;
using TagCloud.App.CloudCreatorDriver.CloudDrawers.DrawingSettings;

namespace TagCloud.App.CloudCreatorDriver.CloudDrawers;

public class CloudDrawer : ICloudDrawer
{
    public Result<Bitmap> DrawWords(List<IDrawingWord> words, IDrawingSettings settings)
    {
        return Result.Of(() =>  FindSizeByRectangles(words))
            .Then(size => Result.FailIf(size, size.Height > settings.PictureSize.Height,
                $"Height {settings.PictureSize.Height} can not be less than required {size.Height}"))
            .Then(size => Result.FailIf(size, size.Width > settings.PictureSize.Width,
                $"Width {settings.PictureSize.Width} can not be less than required {size.Width}"))
            .Then(_ => Draw(words, settings.PictureSize.Width, settings.PictureSize.Height, settings.BgColor));
    }

    private static Size FindSizeByRectangles(IReadOnlyCollection<IDrawingWord> words)
    {
        if (words.Count == 0)
            return new Size(0, 0);
        
        var width = words.Max(word => word.Rectangle.Right);
        var height = words.Max(word => word.Rectangle.Bottom);
        return new Size(width, height);
    }

    private static Bitmap Draw(
        IEnumerable<IDrawingWord> drawingWords,
        int width, int height,
        Color bgColor)
    {
        var myBitmap = new Bitmap(width, height);
        var graphics = Graphics.FromImage(myBitmap);
        graphics.Clear(bgColor);

        foreach (var word in drawingWords)
        {
            graphics.DrawString(
                word.Value,
                word.Font,
                new SolidBrush(word.Color),
                word.Rectangle.Location);
        }

        return myBitmap;
    }
}