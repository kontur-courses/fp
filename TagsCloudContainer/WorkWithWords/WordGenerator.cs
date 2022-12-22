using System.Drawing;
using TagsCloudContainer.Layouter;

namespace TagsCloudContainer.WorkWithWords
{
    public class WordGenerator
    {
        private Settings _settings;
        private CircularCloudLayouter _layouter;
        
        public WordGenerator(CircularCloudLayouter layouter, Settings settings)
        {
            _layouter = layouter;
            _settings = settings;
        }
        
        public List<Rectangle> GenerateRectanglesByWords(List<Word> words)
        {
            var rectangles = new List<Rectangle>();
            foreach (var word in words)
            {
                //Проверка на существуемость
                using var font = new Font(_settings.WordFontName, word.Size);
                var size = MeasureWord(word.Value, font);
                rectangles.Add(_layouter.PutNextRectangle(size));
            }

            return rectangles;
        }

        private static Size MeasureWord(string word, Font font)
        {
            using var bitmap = new Bitmap(1, 1);
            using var graphics = Graphics.FromImage(bitmap);
            var result = graphics.MeasureString(word, font);
            result = result.ToSize();
            if (result.Width == 0) result.Width = 1;
            if (result.Height == 0) result.Height = 1;
            return result.ToSize();
        }
    }
}