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
    
    public Result<Bitmap> CreateImage(List<Word> words)
    {
        return RectangleGenerator.GenerateRectangles(words)
            .Then(rectangles =>
            {
                var bitmap = RectangleExtension.GetBounds(rectangles);
                var shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);
                using var graphics = Graphics.FromImage(bitmap);

                graphics.Clear(Color.Black);
                DrawWordsOnCloud(words, rectangles, shiftToBitmapCenter, graphics);

                return bitmap;
            });
    }

    private void DrawWordsOnCloud(List<Word> words, List<Rectangle> rectangles, Size shiftToBitmapCenter, Graphics graphics)
    {
        using var pen = new Pen(Settings.Color);

        for (var i = 0; i < words.Count; i++)
        {
            var word = words[i];
            var rectangle = new Rectangle(rectangles[i].Location + shiftToBitmapCenter, rectangles[i].Size);
            var isOutOfBitmap = RectangleExtension.IsRectangleOutOfBitmap(rectangle, graphics.VisibleClipBounds);

            if (isOutOfBitmap)
            {
                throw new IndexOutOfRangeException(
                    $"Тэг не влез на изображение заданного размера: X: {Settings.CenterX} Y: {Settings.CenterY}");
            }

            using var font = new Font(Settings.FontName, word.Size);
            graphics.DrawString(word.Value, font, pen.Brush, rectangle);
        }
    }
}
