using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Common;
using TagsCloud.Core;
using TagsCloud.ResultPattern;

namespace TagsCloud.Visualization
{
    public class CloudVisualization
    {
        private static readonly Random Random = new Random();
        private readonly Dictionary<char, Color> letterColor = new Dictionary<char, Color>();

        private readonly FontSettings fontSettings;
        private readonly Palette palette;
        private readonly IImageHolder imageHolder;
        private readonly ColorAlgorithm colorAlgorithm;

        public CloudVisualization(IImageHolder imageHolder, Palette palette, FontSettings fontSettings,
            ColorAlgorithm colorAlgorithm)
        {
            this.colorAlgorithm = colorAlgorithm;
            this.palette = palette;
            this.imageHolder = imageHolder;
            this.fontSettings = fontSettings;
        }

        public void Paint(ICircularCloudLayouter cloud, List<(string, int)> words)
        {
            var newFonts = new Dictionary<int, Font>();
            var rectangles = TagsHelper.GetRectangles(cloud, words, newFonts, fontSettings.MainFont);

            using (var graphics = imageHolder.StartDrawing().GetValueOrThrow())
            {
                var imageSize = imageHolder.GetImageSize().GetValueOrThrow();
                if (!RectanglesFitIntoSize(rectangles, imageSize))
                    Result.Fail<Size>("cloud does not fit into image size").GetValueOrThrow();

                using (var brush = new SolidBrush(palette.BackgroundColor))
                    graphics.FillRectangle(brush, 0, 0, imageSize.Width, imageSize.Height);

                for (var i = 0; i < rectangles.Count; ++i)
                {
                    var drawFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    graphics.DrawRectangle(new Pen(Color.White), rectangles[i]);

                    using (var brush = new SolidBrush(GetColor(words[i].Item1[0])))
                        graphics.DrawString(words[i].Item1, newFonts[words[i].Item2], brush, rectangles[i], drawFormat);
                }
            }

            DisposeFonts(newFonts);
        }

        private void DisposeFonts(Dictionary<int, Font> newFonts)
        {
            foreach (var currentFont in newFonts)
                currentFont.Value.Dispose();
        }

        private Color GetColor(char letter)
        {
            switch (colorAlgorithm.Type)
            {
                case ColorAlgorithmType.MultiColor:
                    return palette.ForeColors[Random.Next(0, palette.ForeColors.Length)];
                case ColorAlgorithmType.SameFirstLetterHasSameColor:
                    if (!letterColor.ContainsKey(letter))
                        letterColor[letter] =
                            Color.FromArgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255));
                    return letterColor[letter];
                default:
                    return palette.ForeColor;
            }
        }

        private bool RectanglesFitIntoSize(List<Rectangle> rectangles, Size size)
        {
            var maxRight = rectangles.Max(x => x.Right);
            var minLeft = rectangles.Min(x => x.Left);
            var maxBottom = rectangles.Max(x => x.Bottom);
            var minTop = rectangles.Min(x => x.Top);

            return maxRight < size.Width
                   && minLeft > 0
                   && maxBottom < size.Height
                   && minTop > 0;
        }
    }
}
