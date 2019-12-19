using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.CloudPainters
{
    public class MultiColorCloudPainter : ICloudPainter<Tuple<string, Rectangle>>
    {
        public Result<Bitmap> GetImage(IEnumerable<Tuple<string, Rectangle>> drawnComponents,
            VisualisingOptions visualisingOptions)
        {
            var field = new Bitmap(visualisingOptions.ImageSize.Width, visualisingOptions.ImageSize.Height);
            using (var graphics = Graphics.FromImage(field))
            {
                graphics.Clear(visualisingOptions.BackgroundColor);
                foreach (var (word, rectangle) in drawnComponents)
                {
                    var color = ColorGenerator.Generate();
                    while (color == visualisingOptions.BackgroundColor)
                        color = ColorGenerator.Generate();
                    var brush = new SolidBrush(color);
                    if(!rectangle.FitsToImage(visualisingOptions.ImageSize))
                        return Result.Fail<Bitmap>("Cloud didn't fit on image");
                    graphics.DrawString(word, visualisingOptions.Font, brush, rectangle.Location);
                }
            }

            return Result.Ok(field);
        }
    }
}