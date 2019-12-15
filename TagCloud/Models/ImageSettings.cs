using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TagCloud.Models
{
    public class ImageSettings
    {
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
            get
            {
                return FieldsToRead.All(fieldIsRead => fieldIsRead.Value);
            }
        }

        public Dictionary<string, bool> FieldsToRead;
        public int Width { get; private set; }
        public int Height { get; set; }
        public string FontName { get; set; }
        public string PaletteName { get; set; }

        private static Dictionary<string, bool> GetFieldsToRead()
        {
            var result = typeof(ImageSettings)
                .GetProperties()
                .Where(p=>p.Name!="IsCompleted")
                .ToDictionary(f=>f.Name,f=>false);
            return result;
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
    }
}