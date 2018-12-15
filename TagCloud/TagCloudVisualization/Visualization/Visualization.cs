using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Forms;
using TagCloud.Settings;

namespace TagCloud.TagCloudVisualization.Visualization
{
    public abstract class Visualization
    {
        protected Graphics Graphics;
        public ImageBox ImageBox;
        protected ImageSettings ImageSettings;
        protected IEnumerable<Rectangle> Rectangles;

        public Rectangle ShiftRectangleToCenter(Rectangle rect)
        {
            var layoutCenter = new Point(ImageSettings.Width / 2, ImageSettings.Height / 2);
            return new Rectangle(new Point(rect.X + layoutCenter.X, rect.Y + layoutCenter.Y), rect.Size);
        }

        public void GetTagCloudImage()
        {
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