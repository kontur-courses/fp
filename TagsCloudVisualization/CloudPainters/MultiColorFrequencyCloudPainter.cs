using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;
using TagsCloudVisualization.WordSizing;

namespace TagsCloudVisualization.CloudPainters
{
    public class MultiColorFrequencyCloudPainter : ICloudPainter<Tuple<SizedWord, Rectangle>>
    {
        public Result<Bitmap> GetImage(IEnumerable<Tuple<SizedWord, Rectangle>> drawnComponents,
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
                    if (rectangle.Location.X + rectangle.Width > visualisingOptions.ImageSize.Width ||
                        rectangle.Location.Y + rectangle.Height > visualisingOptions.ImageSize.Height)
                        return Result.Fail<Bitmap>("Cloud didn't fit on image");
                    graphics.DrawString(word.Value, new Font(visualisingOptions.Font.Name, word.Size), brush,
                        rectangle.Location);
                }
            }

            return Result.Ok(field);
        }
    }
}