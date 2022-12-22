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

        public Bitmap Paint(List<Word> words)
        {
            var rectangles = _wordGenerator.GenerateRectanglesByWords(words);
            var bitmap = GenerateBitmap(rectangles);
            var shiftToBitmapCenter = new Size(bitmap.Width / 2, bitmap.Height / 2);

            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            var count = 0;
            using var pen = new Pen(_settings.WordColor);
            foreach (var word in words)
            {
                var rectangleOnMap = CreateRectangleOnMap(rectangles[count], shiftToBitmapCenter);
                using var font = new Font(_settings.WordFontName, word.Size);
                graphics.DrawString(word.Value, font, pen.Brush, rectangleOnMap.Location);
                count++;
            }

            return bitmap;
        }

        private Rectangle CreateRectangleOnMap(Rectangle rectangle, Size shiftToBitmapCenter)
        {
            return new Rectangle(rectangle.Location + shiftToBitmapCenter, rectangle.Size);
        }
    }
}