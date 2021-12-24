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
        public static Result<Bitmap> GetCloudVisualization(TagCloudParameters parameters)
        {
            if (parameters.Words == null)
            {
                return Result.Fail<Bitmap>("Words can't be null");
            }

            if (parameters.VisualizationParameters.TagColors == null)
            {
                return Result.Fail<Bitmap>("Tags colors can't be null");
            }

            return GenerateRectangles(parameters.Words, parameters.VisualizationParameters.TagSizeRange,
                    parameters.Layouter, parameters.ReductionCoefficient)
                .Then(_ => CloudVisualizer.Draw(parameters.Layouter, parameters.VisualizationParameters.TagColors,
                    parameters.VisualizationParameters.BackgroundColor))
                .ThenDo(bitmap => AddWordsToImage(bitmap, parameters.Layouter.Rectangles, parameters.Words,
                    parameters.VisualizationParameters));
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
            VisualizationParameters parameters)
        {
            if (parameters.MinFontSize <= 0)
            {
                return Result.Fail<None>("Font size can't be zero or negative");
            }

            if (parameters.FontFamily == null)
            {
                return Result.Fail<None>("Font family can't be null");
            }

            if (parameters.TextBrushes == null)
            {
                return Result.Fail<None>("Brushes can't be null");
            }

            if (parameters.TextBrushes.Any(brush => brush == null))
            {
                return Result.Fail<None>("Brush can't be null");
            }

            var rnd = new Random();
            var graphics = Graphics.FromImage(bitmap);
            var result = new Result<float>();
            for (var i = 0; i < words.Count && i < rectangles.Count; i++)
            {
                var brushIndex = parameters.TextBrushes.Count == Math.Min(words.Count, rectangles.Count)
                    ? i
                    : rnd.Next(0, parameters.TextBrushes.Count);

                result = result.Then(_ => GetFontSize(rectangles[i], words[i], parameters.MinFontSize)
                    .ThenDo(fontSize => graphics.DrawString(words[i], new Font(parameters.FontFamily, fontSize),
                        parameters.TextBrushes[brushIndex], rectangles[i])));
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