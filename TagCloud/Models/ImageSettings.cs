using System;
using System.Linq;

namespace TagCloud.Models
{
    public class ImageSettings : ICloneable
    {
        public ImageSettings(int width, int height, string fontName, string paletteName)
        {
            PaletteName = paletteName;
            Width = width;
            Height = height;
            FontName = fontName;
        }

        public ImageSettings(int width, int height, string fontName, string paletteName, string formatName)
        {
            PaletteName = paletteName;
            Width = width;
            Height = height;
            FontName = fontName;
            FormatName = formatName;
        }

        public ImageSettings()
        {
            PaletteName = null;
            Width = 0;
            Height = 0;
            FontName = null;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string FontName { get; set; }
        public string PaletteName { get; set; }
        public string FormatName { get; set; }

        public object Clone()
        {
            return new ImageSettings(Width, Height, FontName, PaletteName);
        }

        public static ImageSettings GetDefaultSettings()
        {
            return new ImageSettings(500, 500, "Arial", "ShadesOfBlue", "png");
        }

        public override int GetHashCode()
        {
            return GetType().GetProperties().Sum(p => p.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var settings = obj as ImageSettings;
            return !(obj is null) &&
                   Width == settings.Width && Height == settings.Height &&
                   FontName == settings.FontName && PaletteName == settings.PaletteName &&
                   FormatName == settings.FormatName;
        }
    }
}