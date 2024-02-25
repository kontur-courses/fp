using System.Drawing;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.DrawRectangle;

public class RectangleDraw : IDraw
{
    private  Settings Settings { get; }
    private  WordRectangleGenerator RectangleGenerator { get; }
    
    public RectangleDraw(WordRectangleGenerator rectangleGenerator, Settings settings)
    {
        RectangleGenerator = rectangleGenerator;
        Settings = settings;
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
        var rectangles = RectangleGenerator.GenerateRectangles(words);
        var bitmap = CreateBitmap(rectangles.Value);
        var shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);
        using var graphics = Graphics.FromImage(bitmap);
        
        if (!rectangles.IsSuccess)
        {
            return Result.Fail<Bitmap>(rectangles.Error);
        }
        graphics.Clear(Color.Black);
        DrawWordsOnCloud(words, rectangles.Value, shiftToBitmapCenter, graphics);
        return bitmap.Ok();
    }

    private void DrawWordsOnCloud(List<Word> words, List<Rectangle> rectangles, Size shiftToBitmapCenter, Graphics graphics)
    {
        var pen = new Pen(Settings.Color);
        var count = 0;
        
        foreach (var word in words)
        {
            var rectangle = new Rectangle(rectangles[count].Location + shiftToBitmapCenter, rectangles[count].Size);
            IsRectangleOutOfBitmap(rectangle, graphics.VisibleClipBounds);
            using var font = new Font(Settings.FontName, word.Size);
            graphics.DrawString(word.Value, font, pen.Brush, rectangle);
            count++;
        }
    }
    
    private void IsRectangleOutOfBitmap(Rectangle rectangle, RectangleF bitmapBounds)
    {
        if(rectangle.Left < 0 || rectangle.Top < 0 || rectangle.Right > bitmapBounds.Width ||
               rectangle.Bottom > bitmapBounds.Height)
        {
            Result.Fail<Bitmap>(
                $"Тэг не влез на изображение заданного размера: X: {Settings.CenterX} Y: {Settings.CenterY}");
        }
    }
}
