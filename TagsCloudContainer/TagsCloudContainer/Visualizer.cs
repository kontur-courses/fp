using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudContainer
{
    public static class Visualizer
    {
        public static Result<Bitmap> GetCloudVisualization(List<string> words, List<Color> tagsColors,
            Color backgroundColor, Size minTagSize, Size maxTagSize, CircularCloudLayouter layouter,
            double reductionCoefficient, float minFontSize, FontFamily fontFamily, Brush textBrush)
        {
            return GetCloudVisualization(words, tagsColors, backgroundColor, minTagSize, maxTagSize, layouter,
                reductionCoefficient, minFontSize, fontFamily, new List<Brush>() {textBrush});
        }

        public static Result<Bitmap> GetCloudVisualization(List<string> words, List<Color> tagsColors,
            Color backgroundColor, Size minTagSize, Size maxTagSize, CircularCloudLayouter layouter,
            double reductionCoefficient, float minFontSize, FontFamily fontFamily, List<Brush> brushes)
        {
            if (words == null)
            {
                return Result.Fail<Bitmap>("Words can't be null");
            }

            if (tagsColors == null)
            {
                return Result.Fail<Bitmap>("Tags colors can't be null");
            }

            return GenerateRectangles(words, minTagSize, maxTagSize, layouter, reductionCoefficient)
                .Then(_ => CloudVisualizer.Draw(layouter, tagsColors, backgroundColor))
                .ThenDo(bitmap =>
                    AddWordsToImage(bitmap, layouter.Rectangles, words, minFontSize, fontFamily, brushes));
        }

        private static Result<None> GenerateRectangles(List<string> words, Size minSize, Size maxSize,
            CircularCloudLayouter layouter, double reductionCoefficient)
        {
            if (maxSize.Height < minSize.Height || maxSize.Width < minSize.Width)
            {
                return Result.Fail<None>("Min tag size must be less or equal than max tag size");
            }

            if (reductionCoefficient is <= 0 or >= 1)
            {
                return Result.Fail<None>("Reduction coefficient must be between 0 and 1");
            }

            if (layouter == null)
            {
                return Result.Fail<None>("Layouter can't be null");
            }

            var currentSize = maxSize;
            var result = Result.Ok(Rectangle.Empty);
            foreach (var _ in words)
            {
                currentSize.Width = currentSize.Width < minSize.Width ? minSize.Width : currentSize.Width;
                currentSize.Height = currentSize.Height < minSize.Height ? minSize.Height : currentSize.Height;
                result = result.Then(_ => layouter.PutNextRectangle(currentSize));
                currentSize.Height = (int) (currentSize.Height * reductionCoefficient);
                currentSize.Width = (int) (currentSize.Width * reductionCoefficient);
            }

            return result
                .Then(_ => new None());
        }

        private static Result<None> AddWordsToImage(Bitmap bitmap, List<Rectangle> rectangles, List<string> words,
            float minFontSize, FontFamily fontFamily, List<Brush> brushes)
        {
            if (minFontSize <= 0)
            {
                return Result.Fail<None>("Font size can't be zero or negative");
            }

            if (fontFamily == null)
            {
                return Result.Fail<None>("Font family can't be null");
            }

            if (brushes == null)
            {
                return Result.Fail<None>("Brushes can't be null");
            }

            if (brushes.Any(brush => brush == null))
            {
                return Result.Fail<None>("Brush can't be null");
            }

            var rnd = new Random();
            var graphics = Graphics.FromImage(bitmap);
            var result = Result.Ok(float.MaxValue);
            for (var i = 0; i < words.Count && i < rectangles.Count; i++)
            {
                var brushIndex = brushes.Count == Math.Min(words.Count, rectangles.Count)
                    ? i
                    : rnd.Next(0, brushes.Count);

                result = result.Then(_ => GetFontSize(rectangles[i], words[i], minFontSize)
                    .ThenDo(fontSize => graphics.DrawString(words[i], new Font(fontFamily, fontSize),
                        brushes[brushIndex], rectangles[i])));
            }

            return result.Then(_ => new None());
        }

        private static Result<float> GetFontSize(Rectangle rectangle, string word, float minFontSize)
        {
            var size = TextRenderer.MeasureText(word, new Font(FontFamily.GenericSansSerif, minFontSize));
            if (size.Width > rectangle.Width || size.Height > rectangle.Height)
            {
                return Result.Fail<float>("Word is too long for tag cloud");
            }

            var currentFontSize = minFontSize;
            while (size.Width <= rectangle.Width && size.Height <= rectangle.Height)
            {
                currentFontSize++;
                size = TextRenderer.MeasureText(word, new Font(FontFamily.GenericSansSerif, currentFontSize));
            }

            return Result.Ok(currentFontSize - 1);
        }
    }
}