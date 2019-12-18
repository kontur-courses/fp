using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Api;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Implementations
{
    [CliElement("wordvisualizer")]
    public class TagCloudVisualizer : IWordVisualizer
    {
        private readonly IWordCloudLayouter layouter;
        private readonly DrawingOptions options;
        private readonly IWordBrushProvider wordBrushProvider;

        public TagCloudVisualizer(IWordCloudLayouter layouter, IWordBrushProvider wordBrushProvider,
            DrawingOptions options)
        {
            this.layouter = layouter;
            this.wordBrushProvider = wordBrushProvider;
            this.options = options;
        }

        public Image CreateImageWithWords(IEnumerable<string> words)
        {
            var layout = new List<Rectangle>();
            var wordsAndCounts = WordCounter.CreateWordOccurrencesDictionary(words);
            var wordsAndRectangles = layouter.AddWords(wordsAndCounts, layout);

            var bmp = layout.CreateSizedBitmap(options.ImageSize);
            var graphics = Graphics.FromImage(bmp);

            FillBackground(graphics, bmp, options);

            WriteWordsOnImage(wordsAndRectangles, wordsAndCounts, graphics, bmp);

            graphics.Flush();
            return bmp;
        }

        private void WriteWordsOnImage(Result<IReadOnlyDictionary<string, Rectangle>> rectangles,
            IReadOnlyDictionary<string, int> counts, Graphics graphics, Image bmp)
        {
            var layoutDimensions = rectangles
                .Then(r => r.Values.ToList().GetLayoutDimensions())
                .DefaultIfError();
            var widthRatio = bmp.Width / (float) layoutDimensions.width;
            var heightRatio = bmp.Height / (float) layoutDimensions.height;

            graphics.ScaleTransform(widthRatio, heightRatio);

            rectangles.Then(d =>
            {
                foreach (var (word, rect) in d)
                {
                    WriteWord(word, rect, counts[word], graphics, layoutDimensions.width, layoutDimensions.width);
                }
            });
        }

        private void WriteWord(string word, Rectangle rect, int count, Graphics graphics, int bmpWidth, int bmpHeight)
        {
            rect.Offset(bmpWidth / 2, bmpHeight / 2);
            graphics.TranslateTransform(rect.X, rect.Y);
            var brush = wordBrushProvider.CreateBrushForWord(word, count);
            graphics.ScaleTransform(count, count);
            graphics.DrawString(word, options.Font, brush, 0, 0);
            graphics.ScaleTransform(1f / count, 1f / count);
            graphics.TranslateTransform(-rect.X, -rect.Y);
        }


        private static void FillBackground(Graphics graphics, Image bmp, DrawingOptions options)
        {
            graphics.FillRegion(options.BackgroundBrush, new Region(new Rectangle(0, 0, bmp.Width, bmp.Width)));
        }
    }
}