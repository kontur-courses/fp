using System.Drawing;

namespace TagCloud
{
    public class Tag
    {
        public string Text { get; }
        public RectangleF LayoutRectangle { get; }
        public Font Font { get; }

        public Tag(string text, RectangleF layoutRectangle, Font font)
        {
            Text = text;
            LayoutRectangle = layoutRectangle;
            Font = font;
        }
    }
}