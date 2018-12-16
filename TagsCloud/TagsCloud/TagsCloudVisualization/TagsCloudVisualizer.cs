using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.TagsCloudVisualization.ColorSchemes;

namespace TagsCloud.TagsCloudVisualization
{

    public class TagsCloudVisualizer: ITagsCloudVisualizer
    {
        public readonly Size PictureSize;
        public readonly string FontName;
        public readonly Color BackgroundColor;
        public readonly IColorScheme ColorScheme;

        public TagsCloudVisualizer(Size pictureSize, string fontName, Color backgroundColor, IColorScheme colorScheme)
        {
            PictureSize = pictureSize;
            FontName = fontName;
            BackgroundColor = backgroundColor;
            ColorScheme = colorScheme;
        }

        public Result<Bitmap> GetCloudVisualization(List<Tag> tags)
        {
            var pictureRectangle = GetPictureRectangle(tags);
            var bitmap = Result.Of(() => new Bitmap(pictureRectangle.Width, pictureRectangle.Height));
            if (!bitmap.IsSuccess)
                return bitmap;
            using (var graphics = Graphics.FromImage(bitmap.Value))
            {
                graphics.Clear(BackgroundColor);
                graphics.TranslateTransform(-pictureRectangle.X, -pictureRectangle.Y);
                foreach (var tag in tags)
                {
                    var color = ColorScheme.DefineColor(tag.Frequency);
                    if (!color.IsSuccess)
                        return Result.Fail<Bitmap>(color.Error);
                    Brush brush = new SolidBrush(color.Value);
                    graphics.DrawString(tag.Word, new Font(FontName, tag.FontSize), brush, tag.PosRectangle.Location);
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
            }

            var  resultBitmap = Result.Of(() => new Bitmap(PictureSize.Width, PictureSize.Height));
            if (!resultBitmap.IsSuccess)
                return resultBitmap.ReplaceError(s=> "Wrong bitmap parameters");
            using (var graphics = Graphics.FromImage(resultBitmap.Value))
            {
                graphics.DrawImage(bitmap.Value, 0, 0, PictureSize.Width, PictureSize.Height);
            }
            return resultBitmap;
        }

        private static Rectangle GetPictureRectangle(List<Tag> tags)
        {
            var rectangles = tags.Select(t => t.PosRectangle);
            var minX = rectangles.Min(rect => rect.Left);
            var minY = rectangles.Min(rect => rect.Top);
            var maxX = rectangles.Max(rect => rect.Right);
            var maxY = rectangles.Max(rect => rect.Bottom);
            return new Rectangle(new Point(minX, minY), new Size(maxX - minX, maxY - minY));
        }

    }
}
