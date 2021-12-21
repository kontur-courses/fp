using System.Drawing;

namespace TagsCloudContainer
{
    public class Word
    {
        public Rectangle Border { get; }
        public string Text { get; }
        public Font Font{ get; }
        public Word(Rectangle border, string text, Font font)
        {
            Border = border;
            Text = text;
            Font = font;
        }
    }
}
