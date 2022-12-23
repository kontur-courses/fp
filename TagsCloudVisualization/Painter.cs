using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ResultOf;
using TagsCloudVisualization.TextFormatters;

namespace TagsCloudVisualization
{
    public class Painter : IPainter
    {
        private readonly IEnumerable<Color> colors;
        private readonly IList<Color> defaultColors = new List<Color> { Color.Aqua };
        private readonly Random rnd;
        private readonly Size size;

        public Painter(Size size) : this(size, new List<Color>())
        {
        }

        public Painter(Size size, IEnumerable<Color> colors)
        {
            this.size = size;
            this.colors = colors;
            rnd = new Random();
        }

        private Color RandomColor
        {
            get
            {
                var colorsList = colors.ToList();
                return colorsList.Count == 0
                    ? defaultColors[rnd.Next(defaultColors.Count)]
                    : colorsList[rnd.Next(colorsList.Count)];
            }
        }

        public Result<None> DrawWordsToFile(List<Word> words, string path)
        {
            return TransformWords(words, size)
                .Then(t =>
                {
                    var b = new Bitmap(size.Width, size.Height);

                    using (var g = Graphics.FromImage(b))
                    {
                        foreach (var word in words)
                        {
                            var stringFormat = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            g.DrawString(word.Value, word.Font, new SolidBrush(RandomColor), word.Rectangle,
                                stringFormat);
                        }
                    }

                    b.Save(path);
                });
        }

        public Result<None> TransformWords(List<Word> rectangles, Size newCanvas)
        {
            return Result.OfAction(() => rectangles.ForEach(t =>
                t.Rectangle = new Rectangle(CalcPositionForCanvas(t.Rectangle.Location, newCanvas), t.Rectangle.Size)));
        }

        public Result<List<Rectangle>> TransformRectangles(List<Rectangle> rectangles, Size newCanvas)
        {
            return Result.Of(() => rectangles.Select(t =>
                new Rectangle(CalcPositionForCanvas(t.Location, newCanvas), t.Size)
            ).ToList());
        }

        private Point CalcPositionForCanvas(Point position, Size imageSize)
        {
            var x = position.X + imageSize.Width / 2;
            var y = position.Y + imageSize.Height / 2;
            return new Point(x, y);
        }

        public Result<None> DrawRectanglesToFile(List<Rectangle> rectangles, string path)
        {
            return TransformRectangles(rectangles, size).Then(t =>
            {
                var b = new Bitmap(size.Width, size.Height);

                using (var g = Graphics.FromImage(b))
                {
                    rectangles.ForEach(t => g.DrawRectangle(new Pen(Brushes.DeepSkyBlue), t));
                }

                return GetImageFormat(path).Then(t => b.Save(path, t));
            });
        }

        public static Result<ImageFormat> GetImageFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension.ToLower() switch
            {
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".ico" => ImageFormat.Icon,
                ".ic" => ImageFormat.Icon,
                ".jpg" => ImageFormat.Jpeg,
                ".jpeg" => ImageFormat.Jpeg,
                _ => Result.Fail<ImageFormat>($"Extension {extension} are not supported!")
            };
        }
    }
}