using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public static class Visualizer
    {
        public static Result<Bitmap> GetCloudVisualization(List<string> words, CircularCloudLayouter layouter,
            double reductionCoefficient, VisualizationParameters parameters)
        {
            if (words == null)
            {
                return Result.Fail<Bitmap>("Words can't be null");
            }

            if (parameters.TagColors == null)
            {
                return Result.Fail<Bitmap>("Tags colors can't be null");
            }

            return GenerateRectangles(words, parameters.TagSizeRange, layouter, reductionCoefficient)
                .Then(_ => CloudVisualizer.Draw(layouter, parameters.TagColors, parameters.BackgroundColor))
                .ThenDo(bitmap => AddWordsToImage(bitmap, layouter.Rectangles, words, parameters.MinFontSize,
                    parameters.FontFamily, parameters.TextBrushes));
        }

        private static Result<None> GenerateRectangles(List<string> words, SizeRange range,
            CircularCloudLayouter layouter, double reductionCoefficient)
        {
            if (range.MaxSize.Height < range.MinSize.Height || range.MaxSize.Width < range.MinSize.Width)
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

            var currentSize = range.MaxSize;
            var result = Result.Ok(Rectangle.Empty);
            foreach (var _ in words)
            {
                currentSize.Width = currentSize.Width < range.MinSize.Width ? range.MinSize.Width : currentSize.Width;
                currentSize.Height = currentSize.Height < range.MinSize.Height
                    ? range.MinSize.Height
                    : currentSize.Height;
                result = result.Then(_ => layouter.PutNextRectangle(currentSize));
                currentSize.Height = (int) (currentSize.Height * reductionCoefficient);
                currentSize.Width = (int) (currentSize.Width * reductionCoefficient);
            }

            return result.Then(_ => new None());
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
            var result = new Result<float>();
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