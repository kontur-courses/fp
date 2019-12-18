using System.Drawing;

namespace TagsCloudContainer.Core
{
    class Tag
    {
        public string Word { get; }
        public Rectangle Rectangle { get; }
        public Font Font { get; }
        public Tag(string word, Rectangle rectangle, Font font)
        {
            Word = word;
            Rectangle = rectangle;
            Font = font;
        }
    }
}