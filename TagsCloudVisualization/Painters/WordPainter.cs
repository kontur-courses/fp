using TagsCloudVisualization.Core;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Utils;

namespace TagsCloudVisualization.Painters
{
    public abstract class WordPainter
    {
        protected readonly Palette palette;

        public WordPainter(Palette palette)
        {
            this.palette = palette;
        }

        public abstract Result<PaintedWord[]> GetPaintedWords(AnalyzedLayoutedText analyzedLayoutedText);
    }
}