using System.Drawing;

namespace TagsCloudContainer
{
    public class TagRectangle
    {
        public string Value { get; }
        public Rectangle Rectangle { get; }
        public Font Font { get; }

        public TagRectangle(string value, Rectangle rectangle, Font font)
        {
            Value = value;
            Rectangle = rectangle;
            Font = font;
        }

        public bool Equals(TagRectangle obj)
        {
            return Value == obj.Value && Rectangle.Equals(obj.Rectangle);
        }
    }
}