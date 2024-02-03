using System.Drawing;

namespace TagCloud.Drawer;

public class Tag
{
    public readonly string Value;
    public Rectangle Position;
    public readonly int FontSize;

    public Tag(string value, Rectangle position, int fontSize)
    {
        Value = value;
        Position = position;
        FontSize = fontSize;
    }
}