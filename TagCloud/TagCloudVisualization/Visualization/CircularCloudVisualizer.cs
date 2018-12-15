using System.Collections.Generic;
using System.Drawing;
using TagCloud.Settings;

namespace TagCloud.TagCloudVisualization.Visualization
{
    public class CircularCloudVisualizer : Visualization
    {
        public CircularCloudVisualizer(ImageSettings imageSettings, IEnumerable<Rectangle> rectangles)
        {
            Rectangles = rectangles;
            ImageSettings = imageSettings;
        }

        protected override void DrawElements()
        {
            foreach (var rectangle in Rectangles)
            {
                var shiftedRectangle = ShiftRectangleToCenter(rectangle);
                Graphics.DrawRectangle(new Pen(Color.Black, 5), shiftedRectangle);
            }
        }
    }
}