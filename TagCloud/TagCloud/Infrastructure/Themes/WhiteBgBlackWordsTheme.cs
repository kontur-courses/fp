using System.Drawing;

namespace TagCloud
{
    public class WhiteBgBlackWordsTheme : ITheme
    {
        public bool IsChecked { get; set; }

        public WhiteBgBlackWordsTheme()
        {
            IsChecked = true;
        }

        public Color BackgroundColor => Color.White;

        public Color GetWordFontColor(WordToken wordToken) => Color.Black;
    }
}
