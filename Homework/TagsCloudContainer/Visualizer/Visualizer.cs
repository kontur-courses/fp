using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Layouter;
using TagsCloudContainer.Visualizer.ColorGenerators;
using TagsCloudContainer.Visualizer.VisualizerSettings;

namespace TagsCloudContainer.Visualizer
{
    public class Visualizer : IVisualizer, IDisposable
    {
        private readonly Bitmap bmp;
        private readonly Font font;
        private readonly Graphics graphics;

        private readonly Size imageSize;
        private readonly ICloudLayouter layouter;
        private readonly IColorGenerator wordsColorsGenerator;

        public Visualizer(IVisualizerSettings settings, ICloudLayouter layouter)
        {
            imageSize = settings.ImageSize;
            bmp = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(bmp);
            graphics.Clear(settings.BackgroundColor);
            wordsColorsGenerator = settings.WordsColorGenerator;
            font = settings.Font;
            this.layouter = layouter;
        }

        public void Dispose()
        {
            font.Dispose();
            graphics.Dispose();
        }

        public Result<Bitmap> Visualize(Dictionary<string, int> freqDict)
        {
            var wordsCount = freqDict.Count;
            var wordsColors = wordsColorsGenerator.GetColors(wordsCount);
            var orderedFreqPairs = freqDict
                .OrderByDescending(kv => kv.Value)
                .ToArray();
            var mostFreq = orderedFreqPairs.First().Value;
            foreach (var (word, freq) in orderedFreqPairs)
            {
                var visualizationResult = VisualizeWord(word, freq, mostFreq, wordsColors.Pop());
                if (!visualizationResult.IsSuccess)
                    return Result.Fail<Bitmap>("Visualization error. " + visualizationResult.Error);
            }

            return bmp;
        }

        private Result<None> VisualizeWord(string word, int freq, int mostFreq, Color color)
        {
            Font newFont = default!;
            Brush brush = default!;
            try
            {
                var freqDelta = mostFreq - freq;
                newFont = new Font(font.FontFamily, Math.Max(font.Size - freqDelta, 5));
                brush = new SolidBrush(color);
                return GetLayoutRectangle(word, newFont)
                    .Then(rect => graphics.DrawString(word, newFont, brush, rect));
            }
            finally
            {
                newFont.Dispose();
                brush.Dispose();
            }
        }

        private Result<Rectangle> GetLayoutRectangle(string word, Font wordFont)
        {
            var rectSize = new Size((int) wordFont.Size * word.Length, wordFont.Height);
            return layouter.PutNextRectangle(rectSize)
                .Then(CheckRectangleOutsideImage);
        }

        private Result<Rectangle> CheckRectangleOutsideImage(Rectangle rect)
        {
            return rect.Left < 0
                   || rect.Right > imageSize.Width
                   || rect.Top < 0
                   || rect.Bottom > imageSize.Height
                ? Result.Fail<Rectangle>("Tag cloud is outside the image")
                : rect;
        }
    }
}