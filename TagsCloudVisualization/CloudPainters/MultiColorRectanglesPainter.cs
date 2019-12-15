using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.CloudPainters
{
    public class MultiColorRectanglesPainter : ICloudPainter<Rectangle>
    {
        public Result<Bitmap> GetImage(IEnumerable<Rectangle> drawnComponents, VisualisingOptions visualisingOptions)
        {
            var field = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(field))
            {
                graphics.Clear(visualisingOptions.BackgroundColor);
                foreach (var rectangle in drawnComponents)
                {
                    var brush = new SolidBrush(ColorGenerator.Generate());
                    if (rectangle.Location.X > visualisingOptions.ImageSize.Width ||
                        rectangle.Location.Y > visualisingOptions.ImageSize.Height)
                        return Result.Fail<Bitmap>("Cloud didn't fit on image");
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            return Result.Ok(field);
        }
    }
}