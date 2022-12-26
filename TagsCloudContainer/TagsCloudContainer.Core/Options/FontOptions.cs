using System.Drawing;

namespace TagsCloudContainer.Core.Options
{
    public class FontOptions 
    {
        public string FontFamily { get; set; }
        public Color FontColor { get; }

        public FontOptions(string fontFamily, Color fontColor)
        {
            FontFamily = fontFamily;
            FontColor = fontColor;
        }
    }

}