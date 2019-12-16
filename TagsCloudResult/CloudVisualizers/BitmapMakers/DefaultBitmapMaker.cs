using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudResult.Properties;

namespace TagsCloudResult.CloudVisualizers.BitmapMakers
{
    public class DefaultBitmapMaker : IBitmapMaker
    {
        public Result<Bitmap> MakeBitmap(IEnumerable<CloudVisualizationWord> words, CloudVisualizerSettings settings)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(settings.Width, settings.Height);
            }
            catch (Exception e)
            {
                return Result.Fail<Bitmap>(e.Message);
            }
            
            var wordsList = words.ToArray();
            var rectangles = wordsList.Select(w => w.Rectangle).ToList();
            var minX = rectangles.OrderBy(rect => rect.X).First().X;
            var minY = rectangles.OrderBy(rect => rect.Y).First().Y;
            var xRatio = GetXRatio(settings, rectangles);
            var yRatio = GetYRatio(settings, rectangles);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.ScaleTransform(xRatio, yRatio);
                g.TranslateTransform(-minX, -minY);
                g.FillRectangle(
                    new SolidBrush(settings.Palette.BackgroundColor),
                    minX,
                    minY,
                    settings.Width * (1 / xRatio),
                    settings.Height * (1 / yRatio));
                if (settings.Palette.IsGradient)
                    DrawGradient(wordsList, settings, g);
                else
                {
                    var brush = new SolidBrush(settings.Palette.PrimaryColor);
                    foreach (var word in wordsList)
                    {
                        var textSize = g.MeasureString(word.Word, settings.Font);
                        WriteWordToRectangle(g, word, textSize, settings.Font, brush);
                    }
                }
            }

            return bitmap;
        }

        private void DrawGradient(
            IEnumerable<CloudVisualizationWord> words,
            CloudVisualizerSettings settings,
            Graphics g)
        {
            var gradientColors = GenerateGradientColors(
                settings.Palette.PrimaryColor,
                settings.Palette.SecondaryColor,
                words.Count()).ToArray();
            var colorCounter = 0;
            foreach (var word in words)
            {
                var currentColor = gradientColors[colorCounter];
                var textSize = g.MeasureString(word.Word, settings.Font);
                var brush = new SolidBrush(currentColor);
                WriteWordToRectangle(g, word, textSize, settings.Font, brush);
                colorCounter++;
            }
        }

        private static IEnumerable<Color> GenerateGradientColors(Color first, Color second, int count)
        {
            int rMax = first.R;
            int rMin = second.R;
            int gMax = first.G;
            int gMin = second.G;
            int bMax = first.B;
            int bMin = second.B;
            
            for (var i = 0; i < count; i++)
            {
                var rAverage = rMin + (rMax - rMin) * i / count;
                var gAverage = gMin + (gMax - gMin) * i / count;
                var bAverage = bMin + (bMax - bMin) * i / count;
                yield return Color.FromArgb(rAverage, gAverage, bAverage);
            }
        }

        private static void WriteWordToRectangle(
            Graphics g,
            CloudVisualizationWord word,
            SizeF textSize,
            Font font,
            Brush brush)
        {
            var state = g.Save();
            g.TranslateTransform(word.Rectangle.Left, word.Rectangle.Top);
            g.ScaleTransform(word.Rectangle.Width * 1.08f / textSize.Width,
                word.Rectangle.Height * 1.1f / textSize.Height);
            g.DrawString(word.Word, font, brush, PointF.Empty);
            g.Restore(state);
        }

        private static float GetXRatio(CloudVisualizerSettings settings, IEnumerable<Rectangle> rectangles)
        {
            var rectanglesEnum = rectangles as Rectangle[] ?? rectangles.ToArray();
            var maxX = rectanglesEnum.OrderBy(rect => rect.Right).Last().Right;
            var minX = rectanglesEnum.OrderBy(rect => rect.X).First().X;
            var xDifference = Math.Abs(maxX - minX);
            return (float) settings.Width / xDifference;
        }
        
        private static float GetYRatio(CloudVisualizerSettings settings, IEnumerable<Rectangle> rectangles)
        {
            var rectanglesEnum = rectangles as Rectangle[] ?? rectangles.ToArray();
            var maxY = rectanglesEnum.OrderBy(rect => rect.Bottom).Last().Bottom;
            var minY = rectanglesEnum.OrderBy(rect => rect.Y).First().Y;
            var yDifference = Math.Abs(maxY - minY);
            return (float) settings.Height / yDifference;
        }
    }
}