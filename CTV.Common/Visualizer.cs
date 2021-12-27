using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CTV.Common.Layouters;
using FunctionalProgrammingInfrastructure;

namespace CTV.Common
{
    public class Visualizer
    {
        private readonly IWordSizer wordSizer;
        private readonly ILayouter layouter;

        public Visualizer(
            IWordSizer wordSizer,
            ILayouter layouter)
        {
            this.wordSizer = wordSizer;
            this.layouter = layouter;
        }

        public Result<Bitmap> Visualize(string[] words, VisualizerSettings settings)
        {
            return
                from bmp in Result.Ok(new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height))
                from g in Result.Ok(Graphics.FromImage(bmp))
                from gWithBackground in DrawBackground(g, settings)
                from sizedWords in CalculateSizedWords(words, settings.TextFont, gWithBackground)
                from gWithDrawerWords in DrawWords(sizedWords, settings, gWithBackground)
                select new Bitmap(bmp, bmp.Size);
        }

        private Result<List<SizedWord>> CalculateSizedWords(string[] words, Font font, Graphics g)
        {
            return Result.Of(() =>
                wordSizer.Convert(words,
                    font,
                    (word, scaledFont) => Size.Ceiling(g.MeasureString(word, scaledFont))));
        }

        private Result<Graphics> DrawWords(List<SizedWord> sizedWords, VisualizerSettings settings, Graphics g)
        {
            layouter.Center = (Point) (settings.ImageSize / 2);

            return sizedWords.OrderBy(x => x.ScaledFont.SizeInPoints)
                .Aggregate(Result.Ok(g),
                    (currentResult, word) =>
                        from current in currentResult
                        from wordLocation in Result.Of(() => layouter.PutNextRectangle(word.WordSize))
                        from _ in ValidateWordIsInsideImageBorders(settings.ImageSize, wordLocation)
                        from gWithStroke in DrawStroke(wordLocation, settings, g)
                        from gWithWord in DrawWord(word, settings, wordLocation, gWithStroke)
                        select gWithWord);
        }

        private Result<Graphics> DrawWord(SizedWord sizedWord,
            VisualizerSettings settings,
            Rectangle wordLocation,
            Graphics g)
        {
            var textBrush = new SolidBrush(settings.TextColor);
            var (word, font, _) = sizedWord;
            return Result
                .OfAction(() => g.DrawString(word, font, textBrush, wordLocation))
                .Then(_ => g);
        }

        private Result<Graphics> DrawStroke(Rectangle wordLocation, VisualizerSettings settings, Graphics g)
        {
            return Result.Ok(new Pen(settings.StrokeColor))
                .Then(pen => g.DrawRectangle(pen, wordLocation))
                .Then(_ => g);
        }

        private Result<Graphics> DrawBackground(Graphics g, VisualizerSettings settings)
        {
            return Result.OfAction(() => g.FillRectangle(new SolidBrush(settings.BackgroundColor),
                    new Rectangle(Point.Empty, settings.ImageSize)))
                .Then(_ => g);
        }

        private static Result<None> ValidateWordIsInsideImageBorders(Size imageSize, Rectangle wordLocation)
        {
            var imageSizeAsRectangle = new Rectangle(new Point(), imageSize);
            return imageSizeAsRectangle.Contains(wordLocation)
                ? Result.Ok()
                : Result.Fail<None>("Part of word was drawn outside the image");
        }
    }
}