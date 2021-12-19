using System.Drawing;

namespace TagCloud.PreLayout
{
    public class Word
    {
        public readonly string Text;

        public Word(string text, Font font)
        {
            Text = text;
            Font = font;
        }

        public Word(Word word, Rectangle rect) : this(word.Text, word.Font)
        {
            Rectangle = rect;
        }

        public Font Font { get; }
        public Rectangle Rectangle { get; }

        public Word WithFontSize(double fontSize)
        {
            return new Word(Text, new Font(Font.FontFamily, (float) fontSize));
        }
    }
}