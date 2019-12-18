using System;
using System.Collections.Generic;
using System.Linq;

namespace TagCloud.Models
{
    public class ImageSettings : ICloneable
    {
        public Dictionary<string, bool> FieldsToRead;

        public ImageSettings(int width, int height, string fontName, string paletteName)
        {
            PaletteName = paletteName;
            Width = width;
            Height = height;
            FontName = fontName;
            FieldsToRead = GetFieldsToRead();
        }

        public ImageSettings()
        {
            PaletteName = null;
            Width = 0;
            Height = 0;
            FontName = null;
            FieldsToRead = GetFieldsToRead();
        }

        public bool IsCompleted
        {
            get { return FieldsToRead.All(fieldIsRead => fieldIsRead.Value); }
        }

        public int Width { get; private set; }
        public int Height { get; set; }
        public string FontName { get; set; }
        public string PaletteName { get; set; }

        private static Dictionary<string, bool> GetFieldsToRead()
        {
            return typeof(ImageSettings)
                .GetProperties()
                .Where(p => p.Name != "IsCompleted")
                .ToDictionary(f => f.Name, f => false);
        }

        public void ReadWidth(int width)
        {
            Width = width;
            FieldsToRead["Width"] = true;
        }

        public void ReadHeight(int height)
        {
            Height = height;
            FieldsToRead["Height"] = true;
        }

        public void ReadFontName(string fontName)
        {
            FontName = fontName;
            FieldsToRead["FontName"] = true;
        }

        public void ReadPaletteName(string paletteName)
        {
            PaletteName = paletteName;
            FieldsToRead["PaletteName"] = true;
        }

        public static ImageSettings GetDefaultSettings()
        {
            return new ImageSettings(500,500,"Arial","ShadesOfBlue");
        }

        public override int GetHashCode()
        {
            return this.GetType().GetProperties().Sum(p => p.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var settings = obj as ImageSettings;
            return !(obj is null) &&
                   Width == settings.Width && Height == settings.Height &&
                   FontName == settings.FontName && PaletteName == settings.PaletteName;
        }

        public object Clone()
        {
            return new ImageSettings(Width,Height,FontName,PaletteName);
        }
    }
}