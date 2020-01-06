using System.Drawing;

namespace TagCloud.Models
{
    public class FontSettings
    {
        public readonly Color color;
        public readonly float defaultFontSize;
        public readonly string fontFamilyName;
        public readonly FontStyle fontStyle;


        public FontSettings(string fontName)
        {
            defaultFontSize = 10;
            fontFamilyName = fontName;
            fontStyle = FontStyle.Italic;
            color = Color.Black;
        }
    }
}