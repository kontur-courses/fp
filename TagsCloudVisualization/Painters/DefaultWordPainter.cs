using TagsCloudVisualization.Core;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Painters
{
    public class DefaultWordPainter : WordPainter
    {
        public DefaultWordPainter(Palette palette) : base(palette)
        {
        }

        public override Result<PaintedWord[]> GetPaintedWords(AnalyzedLayoutedText analyzedLayoutedText)
        {
            return Result.Of(() => PaintWords(analyzedLayoutedText));
        }

        private PaintedWord[] PaintWords(AnalyzedLayoutedText analyzedLayoutedText)
        {
            var words = new PaintedWord[analyzedLayoutedText.Words.Length];
            for (var i = 0; i < words.Length; i++)
                words[i] = new PaintedWord(analyzedLayoutedText.Words[i], palette.FontColor);
            return words;
        }
    }
}