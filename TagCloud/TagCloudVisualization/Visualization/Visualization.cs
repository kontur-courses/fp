using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Forms;
using TagCloud.Settings;
using TagCloud.TagCloudVisualization.Extensions;

namespace TagCloud.TagCloudVisualization.Visualization
{
    public abstract class Visualization
    {
        protected Graphics Graphics;
        public ImageBox ImageBox;
        protected ImageSettings ImageSettings;
        protected IEnumerable<Rectangle> Rectangles;
        private Size BitmapSize { get; set; }

        private void DetermineBitmapSizes()
        {
            var mostDistantRectangle = Rectangles
                .OrderByDescending(rect => rect.GetCircumcircleRadius())
                .FirstOrDefault();
            var circleRadius = mostDistantRectangle.GetCircumcircleRadius();
            var bitmapSide = Math.Max(circleRadius * 2, Math.Max(ImageSettings.Width, ImageSettings.Height));
            BitmapSize = new Size(bitmapSide, bitmapSide);
        }

        public Rectangle ShiftRectangleToCenter(Rectangle rect)
        {
            var layoutCenter = new Point(BitmapSize.Width / 2, BitmapSize.Height / 2);
            return new Rectangle(new Point(rect.X + layoutCenter.X, rect.Y + layoutCenter.Y), rect.Size);
        }

        public void GetTagCloudImage()
        {
            DetermineBitmapSizes();
            ImageSettings.Width = BitmapSize.Width;
            ImageSettings.Height = BitmapSize.Height;
            ImageBox.RecreateImage(ImageSettings);
            if (!Rectangles.Any())
                return;
            using (Graphics = ImageBox.StartDrawing())
            {
                Graphics.Clear(ImageSettings.BackgroundColor);
                DrawElements();
            }
        }

        protected abstract void DrawElements();
    }
}