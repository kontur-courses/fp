using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Drawing
{
    public class TagCloudDrawer : IDrawer
    {
        private readonly IAppSettings appSettings;

        public TagCloudDrawer(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public Result<Bitmap> DrawImage(IEnumerable<Tag> tags)
        {
            if (appSettings.ImageHeight <= 0 || appSettings.ImageWidth <= 0)
                return Result.Fail<Bitmap>("Image width and image height must be greater than zero");

            var tagsList = tags.ToList();
            if (tagsList.Count == 0)
                return Result.Fail<Bitmap>("Tag cloud doesn't contain any tags");

            var center = tagsList.First().Rectangle.Location;

            var upscaleRatio = CalclulateUpscaleRatio(tagsList, appSettings.ImageWidth, appSettings.ImageHeight);
            if (!upscaleRatio.IsSuccess)
                return Result.Fail<Bitmap>(upscaleRatio.Error);

            var image = new Bitmap(appSettings.ImageWidth, appSettings.ImageHeight);
            using var graph = Graphics.FromImage(image);
            graph.Clear(Color.FromName(appSettings.BackgroundColorName));

            using var brush = new SolidBrush(Color.FromName(appSettings.FontColorName));

            graph.TranslateTransform(image.Width / 2 - center.X, image.Height / 2 - center.Y);

            foreach (var tag in tagsList)
            {
                using var upscaledFont = new Font(tag.Font.Name, tag.Font.Size * upscaleRatio.Value);
                var upscaledRectangle = UpscaleRectangle(tag.Rectangle, upscaleRatio.Value);
                graph.DrawString(tag.Word, upscaledFont, brush, upscaledRectangle);
            }

            return Result.Ok(image);
        }

        private static Result<float> CalclulateUpscaleRatio(IReadOnlyCollection<Tag> tags, int imageWidth,
            int imageHeight)
        {
            var cloudSize = CalculateCurrentCloudSize(tags);
            var widthRatio = (double)cloudSize.Width / imageWidth;
            var heightRatio = (double)cloudSize.Height / imageHeight;
            if (widthRatio > 1 || heightRatio > 1)
                return Result.Fail<float>("Tags do not fit in image size");

            var upscaleRatio = Math.Pow(Math.Max(widthRatio, heightRatio), -1) * 0.9;
            return Result.Ok((float)upscaleRatio);
        }

        private static Size CalculateCurrentCloudSize(IReadOnlyCollection<Tag> tags)
        {
            var right = tags.Max(x => x.Rectangle.Right);
            var left = tags.Min(x => x.Rectangle.Left);
            var bottom = tags.Max(x => x.Rectangle.Bottom);
            var top = tags.Min(x => x.Rectangle.Top);

            var width = right - left;
            var height = bottom - top;

            return new Size(width, height);
        }
        
        private static RectangleF UpscaleRectangle(Rectangle rectangle, float scaleRatio)
        {
            var location = new PointF(rectangle.X * scaleRatio, rectangle.Y * scaleRatio);
            var size = new SizeF(rectangle.Size.Width * scaleRatio, rectangle.Size.Height * scaleRatio);

            return new RectangleF(location, size);
        }
    }
}