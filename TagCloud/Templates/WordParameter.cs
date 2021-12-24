using System.Drawing;

namespace TagCloud.Templates;

public class WordParameter
{
    public RectangleF WordRectangleF { get; }
    public string Word { get; }
    public Color Color { get; }
    public Font Font { get; }

    public WordParameter(string word, Font font, Color color, RectangleF wordRectangleF)
    {
        Font = font;
        Color = color;
        WordRectangleF = wordRectangleF;
        Word = word;
    }
}