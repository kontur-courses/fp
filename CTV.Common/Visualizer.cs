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

        public Result<Bitmap> Visualize(string[] words)
        {
            return Result.Ok()
                .InitVariable(() => settings.ImageSize, out var imageSize)
                .InitVariable(() => new Bitmap(imageSize.Width, imageSize.Height), out var bmp)
                .InitVariable(() => Graphics.FromImage(bmp), out var g)
                .Then(_ => DrawBackground(g))
                .Then(_ => wordSizer.Convert(
                    words,
                    settings.TextFont,
                    (word, font) => g.MeasureString(word, font).ToCeilSize()))
                .Then(sizedWords => DrawWords(sizedWords, imageSize, g))
                .Then(_ => new Bitmap(bmp, bmp.Size));
        }

        private Result<Graphics> DrawWords(List<SizedWord> sizedWords, Size imageSize, Graphics g)
        {
            layouter.Center = (Point) (settings.ImageSize / 2);

            return sizedWords.OrderBy(x => x.ScaledFont.SizeInPoints)
                .Aggregate(Result.Ok(g),
                    (current, word) => current
                        .InitVariable(() => layouter.PutNextRectangle(word.WordSize), out var wordLocation)
                        .Then(_ => ValidateWordIsInsideImageBorders(imageSize, wordLocation))
                        .Then(_ => DrawStroke(wordLocation, g))
                        .Then(graphics => DrawWord(word, wordLocation, graphics)));
        }

        private Result<Graphics> DrawWord(SizedWord sizedWord, Rectangle wordLocation, Graphics g)
        {
            var textBrush = new SolidBrush(settings.TextColor);
            var (word, font, _) = sizedWord;
            return Result
                .OfAction(() => g.DrawString(word, font, textBrush, wordLocation))
                .Then(_ => g);
        }

        private Result<Graphics> DrawStroke(Rectangle wordLocation, Graphics g)
        {
            return Result.Ok(new Pen(settings.StrokeColor))
                .Then(pen => g.DrawRectangle(pen, wordLocation))
                .Then(_ => g);
        }

        private Result<Graphics> DrawBackground(Graphics g)
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