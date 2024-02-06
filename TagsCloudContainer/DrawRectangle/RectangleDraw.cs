using System.Drawing;
using TagsCloudContainer.DrawRectangle.Interfaces;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.DrawRectangle;

public class RectangleDraw : IDraw
{
    private readonly Settings _settings;
    private readonly WordRectangleGenerator _rectangleGenerator;
    
    public RectangleDraw(WordRectangleGenerator rectangleGenerator, Settings settings)
    {
        _rectangleGenerator = rectangleGenerator;
        _settings = settings;
    }

    private Bitmap CreateBitmap(List<Rectangle> rectangles)
    {
        var width = rectangles.Max(rectangle => rectangle.Right) -
                    rectangles.Min(rectangle => rectangle.Left);
        var height = rectangles.Max(rectangle => rectangle.Bottom) -
                    rectangles.Min(rectangle => rectangle.Top);
        return new Bitmap(width * 2, height * 2);
    }
    public Result<Bitmap> CreateImage(List<Word> words)
    {
        var rectangles = _rectangleGenerator.GenerateRectangles(words);
        if (!rectangles.IsSuccess)
            return Result.Fail<Bitmap>(rectangles.Error);
        var bitmap = CreateBitmap(rectangles.Value);
        var shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);
        using var g = Graphics.FromImage(bitmap);
        using var pen = new Pen(_settings.Color);
        g.Clear(Color.Black);
        var count = 0;
        foreach (var word in words)
        {
            var rectangle = new Rectangle(
                rectangles.Value[count].Location + shiftToBitmapCenter, 
                rectangles.Value[count].Size);
            if (rectangle.Left < 0
                || rectangle.Right > bitmap.Width
                || rectangle.Bottom > bitmap.Height
                || rectangle.Top < 0)
                return Result.Fail<Bitmap>($"Тэг не влез на изображение заданного размера: X: {_settings.CenterX} Y: {_settings.CenterY}");
            using var font = new Font(_settings.FontName, word.Size);
            g.DrawString(word.Value, font, pen.Brush, rectangle);
            count++;
        }

        return bitmap.Ok();
    }
}