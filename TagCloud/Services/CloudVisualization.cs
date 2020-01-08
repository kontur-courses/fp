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
            return Result.Of(() => GetPaletteOrThrow(imageSettings.PaletteName))
                .Then(palette => GetImageOrThrow(imageSettings,path,palette));
        }

        private Bitmap GetImageOrThrow(ImageSettings imageSettings, string path, Palette palette)
        {
            if(imageSettings.Width<=0)
                throw new ArgumentException("Параметр width не определен");
            if(imageSettings.Height <= 0)
                throw new ArgumentException("Параметр heigth не определен");
            var image = new Bitmap(imageSettings.Width, imageSettings.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                var tagRectanglesResult = cloud.GetRectangles(graphics, imageSettings, path);
                if (!tagRectanglesResult.IsSuccess)
                    throw  new Exception(tagRectanglesResult.Error);
                var rectangles = RectanglesCustomizer.GetRectanglesWithPalette(palette, tagRectanglesResult.Value);
                foreach (var rectangle in rectangles)
                {
                    graphics.DrawString(rectangle.Tag.Text, rectangle.Tag.Font, new SolidBrush(rectangle.Color),
                        rectangle.Area.Location);
                    rectangle.Tag.Font.Dispose();
                }
            }

            return image;
        }

        private Palette GetPaletteOrThrow(string paletteName)
        {
            if (!(paletteName is null) && PaletteDictionary.ContainsKey(paletteName))
                return PaletteDictionary[paletteName];
            throw new ArgumentException("Параметр PaletteName не определен");
        }
    }
}