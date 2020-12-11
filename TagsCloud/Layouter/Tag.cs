using System.Drawing;

namespace TagsCloud.Layouter
{
    public class Tag
    {
        public Rectangle Rectangle { get; set; }
        public string Text { get; }
        public int FontSize { get; }
        public string FontFamily { get; }
        public string FontColor { get; }

        public Tag(string text, int fontSize, string fontFamily, string fontColor)
        {
            Text = text;
            FontSize = fontSize;
            FontFamily = fontFamily;
            FontColor = fontColor;
        }
    }
}