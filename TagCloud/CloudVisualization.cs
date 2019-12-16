using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class CloudVisualization : ICloudVisualization
    {
        private readonly ICloud cloud;

        public CloudVisualization(ICloud cloud, IPaletteDictionaryFactory paletteDictionaryFactory)
        {
            PaletteDictionary = paletteDictionaryFactory.GetPaletteDictioanry();
            this.cloud = cloud;
        }

        public Dictionary<string, Palette> PaletteDictionary { get; }

        public Result<Bitmap> GetAndDrawRectangles(ImageSettings imageSettings, string path = "test.txt")
        {
            var palette = PaletteDictionary[imageSettings.PaletteName];
            var image = new Bitmap(imageSettings.Width, imageSettings.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                var tagRectanglesResult = cloud.GetRectangles(graphics, imageSettings, path);
                if (!tagRectanglesResult.IsSuccess)
                    return Result.Fail<Bitmap>(tagRectanglesResult.Error);
                var rectangles = RectanglesCustomizer.GetRectanglesWithPalette(palette, tagRectanglesResult.Value);
                foreach (var rectangle in rectangles)
                    graphics.DrawString(rectangle.Tag.Text, rectangle.Tag.Font, new SolidBrush(rectangle.Color),
                        rectangle.Area.Location);
            }

            return image;
        }
    }
}