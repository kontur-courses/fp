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
        private readonly TagsHelper tagsHelper;

        public CloudVisualization(IImageHolder imageHolder, Palette palette, FontSettings fontSettings,
            ColorAlgorithm colorAlgorithm, TagsHelper tagsHelper)
        {
            this.colorAlgorithm = colorAlgorithm;
            this.palette = palette;
            this.imageHolder = imageHolder;
            this.fontSettings = fontSettings;
            this.tagsHelper = tagsHelper;
        }

        public Result<Graphics> Paint(ICircularCloudLayouter cloud, List<(string, int)> words)
        {
            return imageHolder.StartDrawing()
                .Then(graphics => 
                {
                    using var _ = graphics;
                    return PaintBackground(_)
                        .Then(g => fontSettings.Font
                                .Then(font => new DisposableDictionary<int, Font>(tagsHelper.CreateFonts(words, font)))
                                .Then(newFonts =>
                                {
                                    using var localFonts = newFonts;
                                    return tagsHelper.GetRectangles(cloud, words, localFonts)
                                        .Then(RectanglesFitIntoSize)
                                        .Then(x => PaintRectangles(g, x))
                                        .Then(x => PaintWords(g, localFonts, x, words));
                                })
                        );
                });
        }

        private Result<Graphics> PaintBackground(Graphics graphics)
        {
            return imageHolder.GetImageSize()
                .Then(size =>
                {
                    using (var brush = new SolidBrush(palette.BackgroundColor))
                        graphics.FillRectangle(brush, 0, 0, size.Width, size.Height);
                    return graphics;
                });
        }

        private Result<List<Rectangle>> PaintRectangles(Graphics graphics, List<Rectangle> rectangles)
        {
            foreach (var rect in rectangles)
                graphics.DrawRectangle(new Pen(Color.White), rect);
            return rectangles;
        }

        private Result<Graphics> PaintWords(Graphics graphics, DisposableDictionary<int, Font> newFonts,
            List<Rectangle> rectangles, List<(string, int)> words)
        {
            var drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            for (var i = 0; i < rectangles.Count; ++i)
            {
                using (var brush = new SolidBrush(GetColor(words[i].Item1[0])))
                    graphics.DrawString(words[i].Item1, newFonts[words[i].Item2], brush, rectangles[i], drawFormat);
            }

            return graphics;
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

        private Result<List<Rectangle>> RectanglesFitIntoSize(List<Rectangle> rectangles)
        {
            return imageHolder.GetImageSize()
                .Then(size =>
                {
                    return Validate(rectangles,
                        rect => rect.Max(x => x.Right) < size.Width
                                && rect.Min(x => x.Left) > 0
                                && rect.Max(x => x.Bottom) < size.Height
                                && rect.Min(x => x.Top) > 0,
                        "cloud does not fit into image size");
                });
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);
        }
    }
}