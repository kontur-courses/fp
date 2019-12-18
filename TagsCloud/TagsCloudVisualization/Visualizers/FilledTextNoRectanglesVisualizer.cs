using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using TagCloudResult;
using TagsCloudVisualization.Styling;
using TagsCloudVisualization.Styling.Themes;

namespace TagsCloudVisualization.Visualizers
{
    public class TextNoRectanglesVisualizer : ICloudVisualizer
    {
        public Result<Bitmap> Visualize(ITheme theme, IEnumerable<RectangleF> rectangles,
            int width = 1000, int height = 1000)
        {
            var result = new Bitmap(width, height);
            var bordersRectangle = new RectangleF(0, 0, width, height);
            var random = new Random();
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.Clear(DrawingUtils.GetBrushFromHexColor(theme.BackgroundColor).Color);
                foreach (var rectangle in rectangles)
                {
                    if (!bordersRectangle.Contains(rectangle))
                        return Result.Fail<Bitmap>(
                            "Tag cloud went out of image bounds, please specify bigger image or decrease font size");
                    graphics.FillRectangle(GetRandomBrush(random, theme.TextColors), rectangle);
                }

                return Result.Ok(result);
            }
        }

        private static Brush GetRandomBrush(Random random, string[] brushes)
        {
            return DrawingUtils.GetBrushFromHexColor(brushes[random.Next(0, brushes.Length)]);
        }

        public Result<Bitmap> Visualize(Style style, IEnumerable<Tag> tags,
            int width = 1000, int height = 1000)
        {
            var result = new Bitmap(width, height);
            var bordersRectangle = new RectangleF(0, 0, width, height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.Clear(DrawingUtils.GetBrushFromHexColor(style.Theme.BackgroundColor).Color);
                foreach (var tag in tags)
                {
                    if (!bordersRectangle.Contains(tag.Area))
                        return Result.Fail<Bitmap>(
                            "Tag cloud went out of image bounds, please specify bigger image or decrease font size");
                    var tagResult = DrawTag(style, tag, graphics);
                    if(!tagResult.IsSuccess)
                        return Result.Fail<Bitmap>(tagResult.Error);
                }
                return result;
            }
        }

        private static Result<None> DrawTag(Style style, Tag tag, Graphics graphics)
        {
            var pathResult = GenerateTagPath(style, tag);
            if (!pathResult.IsSuccess)
                return Result.Fail<None>(pathResult.Error);
            
            using (var path = pathResult.GetValueOrThrow())
            {
                var transformMatrix = new[]
                {
                    new PointF(tag.Area.Left, tag.Area.Top),
                    new PointF(tag.Area.Right, tag.Area.Top),
                    new PointF(tag.Area.Left, tag.Area.Bottom)
                };
                graphics.Transform = new Matrix(path.GetBounds(), transformMatrix);

                var tagBrush =
                    DrawingUtils.GetBrushFromHexColor(style.TagColorizer.GetTagColor(style.Theme.TextColors, tag));

                graphics.FillPath(tagBrush, path);
            }
            return Result.Ok();
        }

        private static Result<GraphicsPath> GenerateTagPath(Style style, Tag tag)
        {
            var fontResult = style.TagSizeCalculator.GetScaleFactor(tag.Count, style.FontProperties.MinSize)
                .Then(s => style.FontProperties.CreateFont(s));
            if (!fontResult.IsSuccess)
                return Result.Fail<GraphicsPath>(fontResult.Error);

            using (var font = fontResult.GetValueOrThrow())
            {
                var formatCentered = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center,
                };
                var path = new GraphicsPath();
                path.AddString(
                    tag.Word,
                    font.FontFamily,
                    (int) font.Style,
                    tag.Area.Height,
                    new PointF(0, 0),
                    formatCentered);
                return Result.Ok(path);
            }
        }
    }
}