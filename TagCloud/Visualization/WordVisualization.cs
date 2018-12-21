using System.Drawing;

namespace TagCloud
{
    public class WordVisualization
    {
        public string Value { get; }

        public Rectangle Position { get; }

        public Font Font { get; }

        public WordVisualization(string value, Rectangle position, Font font)
        {
            Value = value;
            Position = position;
            Font = font;
        }
    }
}
