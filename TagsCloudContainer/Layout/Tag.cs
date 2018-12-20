using System.Drawing;

namespace TagsCloudContainer.Layout
{
    public struct Tag
    {
        public readonly string Word;
        public readonly Font Font;
        public readonly Rectangle ContainingRectangle;

        public Tag(string word, Font font, Rectangle containingRectangle)
        {
            Word = word;
            Font = font;
            ContainingRectangle = containingRectangle;
        }
    }
}