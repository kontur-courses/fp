using System.Drawing;

namespace TagsCloud.FontGenerators
{
    public class FontSettings
    {
        public readonly FontFamily fontFamily;
        public readonly float fontSize;

        public FontSettings(FontFamily fontFamily, float fontSize)
        {
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;
        }
    }
}