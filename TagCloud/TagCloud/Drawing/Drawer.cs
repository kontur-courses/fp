using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagCloud.Extensions;
using TagCloud.PreLayout;

namespace TagCloud.Drawing
{
    internal class Drawer : IDrawer
    {
        private readonly IPalette _palette;

        public Drawer(IPalette palette)
        {
            _palette = palette;
        }

        public Result<Bitmap> Draw(IDrawerOptions options, List<Result<Word>> wordResults)
        {
            var words = wordResults
                .Where(r => r.IsSuccess)
                .Select(r => r.Value);
            var bitmapResult = GetClearBitmap(options, words.Select(w => w.Rectangle));
            if (!bitmapResult.IsSuccess)
                return Result.Fail<Bitmap>($"Не удалось нарисовать изображение {bitmapResult.Error}");
            var bitmap = bitmapResult.Value;
            using var g = Graphics.FromImage(bitmap);
            _palette
                .WithWordColors(options.WordColors.ToList())
                .WithBackGroundColor(options.BackgroundColor);
            using var backgroundBrush = new SolidBrush(_palette.BackgroundColor);
            g.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);

            var bitmapCenter = new Point(bitmap.Width / 2, bitmap.Height / 2);
            var offsetPoint = new Point(bitmapCenter.X - options.Center.X, bitmapCenter.Y - options.Center.Y);
            foreach (var word in words)
            {
                var rectangle = word.Rectangle;
                using var brush = new SolidBrush(_palette.GetNextColor());
                rectangle.Offset(offsetPoint);
                g.DrawString(word.Text, word.Font, brush, rectangle.Location);
            }

            return bitmap.AsResult();
        }

        private static Result<Bitmap> GetClearBitmap(IDrawerOptions options, IEnumerable<Rectangle> rectangles)
        {
            return GetCanvasSize(rectangles, options.Center)
                .Then(s => new Bitmap(s.Width, s.Height));
        }

        private static Result<Size> GetCanvasSize(IEnumerable<Rectangle> rectangles, Point center)
        {
            var union = rectangles.Aggregate(Rectangle.Union);
            var distancesResult = union.GetDistancesToInnerPoint(center);
            if (!distancesResult.IsSuccess)
                return Result.Fail<Size>($"Не удалось задать размер изображения, {distancesResult.Error}");
            var distances = distancesResult.Value;
            var horizontalIncrement = Math.Abs(distances[0] - distances[2]);
            var verticalIncrement = Math.Abs(distances[1] - distances[3]);
            union.Inflate(horizontalIncrement, verticalIncrement);
            var canvasSize = new Size((int) (union.Width * 1.1), (int) (union.Height * 1.1));
            return canvasSize.AsResult();
        }
    }
}