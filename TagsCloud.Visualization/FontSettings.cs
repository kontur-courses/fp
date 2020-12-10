using System.Drawing;
using TagsCloud.ResultPattern;

namespace TagsCloud.Visualization
{
    public class FontSettings
    {
        private int minFontSize = 8;
        private int maxFontSize = 50;

        private int fontSize;
        private string fontFamilyName;
        private Result<Font> font;

        public string FontFamilyName
        {
            get => fontFamilyName;
            set
            {
                fontFamilyName = value;
                font = Result.Of(() => new Font(new FontFamily(fontFamilyName), fontSize));
            }
        }

        public int FontSize
        {
            get => fontSize;
            set
            {
                if (value < minFontSize)
                    fontSize = minFontSize;
                else if (value > maxFontSize)
                    fontSize = maxFontSize;
                else fontSize = value;

                font = Result.Of(() => new Font(new FontFamily(fontFamilyName), fontSize));
            }
        }

        public Font MainFont => font.GetValueOrThrow();
    }
}