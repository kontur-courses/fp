using System.Drawing;
using TagsCloudContainer.WorkWithWords;

namespace TagsCloudContainer.Visualisators
{
    public class RectangleVisualisator : IVisualisator
    {
        private readonly WordGenerator _wordGenerator;
        private readonly Settings _settings;

        public RectangleVisualisator(WordGenerator wordGenerator, Settings settings)
        {
            _wordGenerator = wordGenerator;
            _settings = settings;
        }

        private Bitmap GenerateBitmap(List<Rectangle> rectangles)
        {
            var width = rectangles.Max(rectangle => rectangle.Right) -
                        rectangles.Min(rectangle => rectangle.Left);

            var height = rectangles.Max(rectangle => rectangle.Bottom) -
                         rectangles.Min(rectangle => rectangle.Top);

            return new Bitmap(width * 2, height * 2);
        }

        public Result<Bitmap> Paint(List<Word> words)
        {
            var rectangles = _wordGenerator.GenerateRectanglesByWords(words);
            if (!rectangles.IsSuccess)
                return Result.Fail<Bitmap>(rectangles.Error);
            var bitmap = GenerateBitmap(rectangles.Value);
            var shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);

            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            var count = 0;
            using var pen = new Pen(_settings.WordColor);
            foreach (var word in words)
            {
                var rectangleOnMap = CreateRectangleOnMap(rectangles.Value[count], shiftToBitmapCenter);
                if(rectangleOnMap.Left < 0 
                   || rectangleOnMap.Right > bitmap.Width 
                   || rectangleOnMap.Bottom > bitmap.Height
                   || rectangleOnMap.Top < 0)
                    return Result.Fail<Bitmap>(
                        $"Tags got out of the image: X:{_settings.CenterX}, Y:{_settings.CenterY}");
                using var font = new Font(_settings.WordFontName, word.Size);
                graphics.DrawString(word.Value, font, pen.Brush, rectangleOnMap.Location);
                count++;
            }

            return bitmap.Ok();
        }

        private Rectangle CreateRectangleOnMap(Rectangle rectangle, Size shiftToBitmapCenter)
        {
            return new Rectangle(rectangle.Location + shiftToBitmapCenter, rectangle.Size);
        }
    }
}