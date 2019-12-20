using System;
using System.ComponentModel;
using System.Drawing;

namespace TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace
{
    public class CloudViewConfiguration
    {
        public double ScaleCoefficient { get; set; }
        public Size ImageSize { get; set; }

        [Browsable(false)] public Result<FontFamily> FontFamily { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }

        public Brush GetBrush()
        {
            return new SolidBrush(TextColor);
        }


        public CloudViewConfiguration()
        {
            ScaleCoefficient = 10;
            ImageSize = new Size(600, 300);
            try
            {
                FontFamily = Result.Ok(new FontFamily("Algerian"));
            }
            catch (Exception)
            {
                FontFamily = Result.Ok(System.Drawing.FontFamily.GenericSansSerif);
            }

            BackgroundColor = Color.Green;
            TextColor = Color.White;
        }

        public bool SetFontFamily(string fontFamily)
        {
            FontFamily = Result.Of(() => new FontFamily(fontFamily), "Font is not found");
            return FontFamily.IsSuccess;
        }
    }
}