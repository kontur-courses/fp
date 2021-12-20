using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using CTV.Common;
using CTV.Common.Layouters;

namespace CTV.Common
{
    public class Visualizer
    {
        private readonly IWordSizer wordSizer;
        private readonly VisualizerSettings settings;
        private readonly ILayouter layouter;

        public Visualizer(
            IWordSizer wordSizer,
            VisualizerSettings settings,
            ILayouter layouter)
        {
            this.wordSizer = wordSizer;
            this.settings = settings;
            this.layouter = layouter;
        }

        public Bitmap Visualize(string[] words)
        {
            var imageSize = settings.ImageSize;
            using var bmp = new Bitmap(imageSize.Width, imageSize.Height);
            using var g = Graphics.FromImage(bmp);
            var sizedWords = wordSizer.Convert(words, settings.TextFont, g);
            
            DrawBackground(g);
            DrawWords(sizedWords, g);
            return new Bitmap(bmp, bmp.Size);
        }

        private void DrawWords(List<SizedWord> sizedWords, Graphics g)
        {
            layouter.Center = (Point) (settings.ImageSize / 2);

            foreach (var sizedWord in sizedWords)
            {
                var wordLocation = layouter.PutNextRectangle(sizedWord.WordSize);
                DrawStroke(wordLocation, g);
                DrawWord(sizedWord, wordLocation, g);
            }
        }

        private void DrawWord(SizedWord sizedWord, Rectangle wordLocation, Graphics g)
        {
            var textBrush = new SolidBrush(settings.TextColor);
            var (word, font, _) = sizedWord;
            g.DrawString(word, font, textBrush, wordLocation);
        }
        
        private void DrawStroke(Rectangle wordLocation, Graphics g)
        {
            var strokePen = new Pen(settings.StrokeColor);
            g.DrawRectangle(strokePen, wordLocation);
        }

        private void DrawBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(settings.BackgroundColor),
                new Rectangle(Point.Empty, settings.ImageSize));
        }
    }
}