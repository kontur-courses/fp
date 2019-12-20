using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FunctionalTools;
using TagsCloudGenerator.CloudLayouter;
using Font = System.Drawing.Font;

namespace TagsCloudGenerator.Visualizer
{
    public class CloudVisualizer : ICloudVisualizer
    {
        private readonly IColoringAlgorithm coloringAlgorithm;
        private readonly ImageSettings imageSettings;

        public CloudVisualizer(IColoringAlgorithm coloringAlgorithm, ImageSettings imageSettings)
        {
            this.coloringAlgorithm = coloringAlgorithm;
            this.imageSettings = imageSettings;
        }

        public Result<Bitmap> Draw(Cloud cloud)
        {
            return Draw(cloud.Words).RefineError("Drawing error");
        }

        private Result<Bitmap> Draw(IReadOnlyCollection<Word> words)
        {
            return words.Count != 0
                ? GetBitmap(words).AsResult()
                : Result.Fail<Bitmap>("No words to draw");
        }

        private Bitmap GetBitmap(IReadOnlyCollection<Word> words)
        {
            var bitmap = new Bitmap(imageSettings.Width, imageSettings.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            using (var colors = coloringAlgorithm.GetColors(imageSettings).GetEnumerator())
            {
                var scale = ComputeScale(words, imageSettings);

                graphics.Clear(imageSettings.BackgroundColor);
                graphics.TranslateTransform(imageSettings.Width / 2f, imageSettings.Height / 2f);
                graphics.ScaleTransform(scale, scale);

                DrawWords(words, colors, graphics);
            }
            
            return bitmap;
        }

        private void DrawWords(IReadOnlyCollection<Word> words, IEnumerator<Color> colors, Graphics graphics)
        {
            foreach (var word in words)
            {
                colors.MoveNext();

                var color = colors.Current;
                var brush = new SolidBrush(color);
                var font = new Font(imageSettings.Font.FontFamily, imageSettings.Font.Size * word.Count);

                graphics.DrawString(word.Value, font, brush, word.Rectangle);
            }
        }

        private static float ComputeScale(IReadOnlyCollection<Word> words, ImageSettings settings)
        {
            var width = settings.Width / GetDispersionOfX(words);
            var height = settings.Height / GetDispersionOfY(words);

            return Math.Min(width, height);
        }

        private static float GetDispersionOfX(IReadOnlyCollection<Word> words)
        {
            var min = words.Min(t => t.Rectangle.Left);
            var max = words.Max(t => t.Rectangle.Right);

            return GetDispersion(min, max);
        }

        private static float GetDispersionOfY(IReadOnlyCollection<Word> words)
        {
            var min = words.Min(t => t.Rectangle.Bottom);
            var max = words.Max(t => t.Rectangle.Top);

            return GetDispersion(min, max);
        }

        private static float GetDispersion(float min, float max)
        {
            var length = max - min;
            var bound = length * 0.1f;

            return length + bound;
        }
    }
}