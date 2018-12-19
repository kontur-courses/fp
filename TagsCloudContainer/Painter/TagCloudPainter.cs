using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Painter
{
    public class TagCloudPainter
    {
        private readonly FontSettings fontSettings;
        private readonly IImageHolder holder;
        private readonly ImageSettings imageSettings;
        private readonly Palette palette;

        public TagCloudPainter(IImageHolder holder,
            ImageSettings imageSettings,
            Palette palette,
            FontSettings fontSettings)
        {
            this.holder = holder;
            this.imageSettings = imageSettings;
            this.palette = palette;
            this.fontSettings = fontSettings;
        }

        public Result<None> Paint(Point center, WordInfo[] wordInfos)
        {
            var radius = (int) wordInfos.Select(wordInfo => wordInfo.Rect)
                .Select(rect => Math.Ceiling(rect.Location.DistanceTo(center))).Max();
            var bitmapSize = GetBitmapSize(wordInfos.Select(info => info.Rect), center);
            return holder.GetImageSize()
                .Then(imageSize => CheckImageSize(imageSize, bitmapSize))
                .Then(imageSize => DrawPicture(imageSize, center, radius, wordInfos))
                .RefineError("Can't draw picture");
        }

        private Result<None> DrawPicture(Size imageSize, Point center, int radius, IEnumerable<WordInfo> wordInfos)
        {
            Result<None> result;
            using (var graphics = holder.StartDrawing().GetValueOrThrow())
            {
                var deltaX = imageSize.Width / 2;
                var deltaY = imageSize.Height / 2;
                graphics.FillRectangle(new SolidBrush(palette.BackgroundColor), 0, 0, imageSize.Width,
                    imageSize.Height);
                graphics.TranslateTransform(deltaX, deltaY);
                result = Result.OfAction(() =>
                {
                    Debug.Assert(graphics != null, nameof(graphics) + " != null");
                    Draw(graphics, wordInfos, center, radius);
                }).RefineError("Invalid Draw Settings");
            }

            return result;
        }

        private void Draw(Graphics graphics, IEnumerable<WordInfo> wordInfos, Point center, int radius)
        {
            foreach (var wordInfo in wordInfos)
            {
                var color = imageSettings.GetCloudPainterClass()
                    .GetRectangleColor(center, wordInfo.Rect, radius);
                graphics.DrawString(
                    wordInfo.Word,
                    new Font(fontSettings.Font.FontFamily, wordInfo.FontSize),
                    new SolidBrush(color),
                    wordInfo.Rect);
            }
        }

        private Result<Size> CheckImageSize(Size imageSize, int bitmapSize)
        {
            if (imageSize.Width < bitmapSize || imageSize.Height < bitmapSize)
                return Result.Fail<Size>("picture size is bigger than window size");
            return imageSize;
        }

        private int GetBitmapSize(IEnumerable<Rectangle> rectangles, Point rectanglesCenter)
        {
            var leftAndRightBorders = rectangles
                .Select(rectangle => (rectangle.Location.X, rectangle.Location.X + rectangle.Width)).ToArray();
            var maxX = leftAndRightBorders.Max(borders => borders.Item2);
            var minX = leftAndRightBorders.Min(borders => borders.Item1);
            var size = maxX - minX + Math.Max(rectanglesCenter.X, rectanglesCenter.Y) * 2 + 50;
            return size;
        }
    }
}