using System;
using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.Visualizators
{
    public class TagsVisualizator : IVisualizator
    {
        public void Visualize(IVisualizatorSettings settings, ICloud cloud)
        {
            var bitmapSize = settings.BitmapSize;
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            var k = CalculateScaleModifier(cloud, bitmapSize, settings.MinMargin);

            graphics.TranslateTransform(bitmapSize.Width / 2, bitmapSize.Height / 2);
            graphics.ScaleTransform(k, k);

            graphics.Clear(settings.BackgroundColor);

            if (settings.FillTags)
                DrawTagRectangles(cloud, graphics);
            VisualizeCenter(cloud, graphics);
            DrawTagTexts(settings, cloud, graphics);

            bitmap.Save(settings.Filename);
        }

        private float CalculateScaleModifier(ICloud cloud, Size bitmapSize, float minMargin)
        {
            var cloudBoundingRectangle = cloud.GetCloudBoundingRectangle();
            var imageBoundingRectangle = RectangleFExtensions
                .GetRectangleByCenter(bitmapSize, new PointF());
            var offset = imageBoundingRectangle
                .GetRectanglesBoundsMaxOffset(cloudBoundingRectangle);
            var offsetWidth = offset.X + minMargin;
            var offsetHeight = offset.Y + minMargin;
            return Math.Min(1, Math.Min(
                bitmapSize.Width / (bitmapSize.Width + 2 * offsetWidth),
                bitmapSize.Height / (bitmapSize.Height + 2 * offsetHeight)));
        }

        private void VisualizeCenter(ICloud cloud,Graphics graphics)
        {
            var brush = new SolidBrush(Color.Gray);
            var centerRect = RectangleFExtensions
                .GetRectangleByCenter(new Size(8, 8), cloud.Center);
            graphics.FillEllipse(brush, centerRect);
        }

        private void DrawTagRectangles(ICloud cloud, Graphics graphics)
        {
            foreach (var tag in cloud.Elements) 
                graphics.FillRectangle(tag.Palette.BackgroundColor, tag.Layout);
        }

        private void DrawTagTexts(IVisualizatorSettings settings, ICloud cloud, Graphics graphics)
        {
            foreach (var tag in cloud.Elements)
            {
                var font = new Font(settings.FontFamily, 12);
                font = GetScaledFont(tag.Text, font, tag.Layout.Size, graphics);
                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                graphics.DrawString(tag.Text, font, tag.Palette.TextColor, tag.Layout, format);
            }
        }

        private Font GetScaledFont(string text, Font font, SizeF layout, Graphics graphics)
        {
            var size = graphics.MeasureString(text, font);
            var width = Math.Min(layout.Width, size.Width);
            var height = Math.Min(layout.Height, size.Height);
            var fontSizeScale = Math.Min(layout.Width / width, layout.Height / height);
            return new Font(font.FontFamily, font.Size * fontSizeScale);
        }
    }
}
