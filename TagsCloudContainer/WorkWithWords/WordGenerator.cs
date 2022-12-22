using System.Drawing;
using TagsCloudContainer.Layouter;

namespace TagsCloudContainer.WorkWithWords
{
    public class WordGenerator
    {
        private Settings _settings;
        private CircularCloudLayouter _layouter;
        private FontProvider _fontProvider;
        
        public WordGenerator(CircularCloudLayouter layouter, 
            Settings settings, FontProvider fontProvider)
        {
            _layouter = layouter;
            _settings = settings;
            _fontProvider = fontProvider;
        }
        
        public Result<List<Rectangle>> GenerateRectanglesByWords(List<Word> words)
        {
            var rectangles = new List<Rectangle>();
            var settingsFont = _fontProvider.TryGetFont(_settings.WordFontName, _settings.WordFontSize);
            if (!settingsFont.IsSuccess)
                return Result.Fail<List<Rectangle>>(settingsFont.Error);
            foreach (var word in words)
            {
                using var font = new Font(settingsFont.Value.FontFamily, word.Size);
                var size = MeasureWord(word.Value, font);
                rectangles.Add(_layouter.PutNextRectangle(size));
            }

            return rectangles.Ok();
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