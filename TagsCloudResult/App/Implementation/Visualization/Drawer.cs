using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using App.Implementation.Words.Tags;
using App.Infrastructure.SettingsHolders;
using App.Infrastructure.Visualization;

namespace App.Implementation.Visualization
{
    public class Drawer : IDrawer
    {
        private const int LineWidth = 2;
        private readonly IImageSizeSettingsHolder imageSizeSettings;

        private readonly IPaletteSettingsHolder paletteSettings;

        public Drawer(
            IPaletteSettingsHolder paletteSettings,
            IImageSizeSettingsHolder imageSizeSettings)
        {
            this.paletteSettings = paletteSettings;
            this.imageSizeSettings = imageSizeSettings;
        }

        public void DrawCanvasBoundary(Image image)
        {
            var boundary = new Rectangle(
                Point.Empty,
                new Size(image.Width - 1, image.Height - 1));

            using var pen = new Pen(Brushes.Red, LineWidth);
            using var graphics = Graphics.FromImage(image);

            graphics.DrawRectangle(pen, boundary);
        }

        public void DrawAxis(Image image, Point cloudCenter)
        {
            using var pen = new Pen(Brushes.Black, LineWidth);
            using var graphics = Graphics.FromImage(image);

            graphics.DrawLine(pen, cloudCenter, new Point(cloudCenter.X, 0));
            graphics.DrawLine(pen, cloudCenter, new Point(cloudCenter.X, image.Height));

            graphics.DrawLine(pen, cloudCenter, new Point(0, cloudCenter.Y));
            graphics.DrawLine(pen, cloudCenter, new Point(image.Width, cloudCenter.Y));
        }

        public void DrawCloudBoundary(Image image, Point cloudCenter, int cloudCircleRadius)
        {
            var size = new Size(cloudCircleRadius * 2, cloudCircleRadius * 2);
            var location = new Point(cloudCenter.X - cloudCircleRadius, cloudCenter.Y - cloudCircleRadius);

            using var pen = new Pen(Brushes.DodgerBlue, LineWidth);
            using var graphics = Graphics.FromImage(image);

            graphics.DrawEllipse(pen, new Rectangle(location, size));
        }

        public void DrawTags(Image image, IEnumerable<Tag> tags)
        {
            using var graphics = Graphics.FromImage(image);
            using var backgroundBrush = new SolidBrush(paletteSettings.BackgroundColor);
            using var brush = new SolidBrush(paletteSettings.WordColor);

            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var backgroundRectangle = new RectangleF(PointF.Empty, new Size(
                imageSizeSettings.Size.Width,
                imageSizeSettings.Size.Height));

            graphics.FillRectangle(backgroundBrush, backgroundRectangle);


            foreach (var tag in tags)
            {
                using var font = new Font(Tag.WordFont.Name, tag.WordEmSize);
                graphics.DrawString(tag.Word, font, brush, tag.WordOuterRectangle.Location);
            }
        }
    }
}