using System.Drawing;

namespace TagCloudResult.Extensions;

public static class FontExtensions
{
    public static Font ChangeSize(this Font font, float fontSize)
    {
        if (font.Size == fontSize)
            return font;
        var fontWithNewSize = new Font(font.Name, fontSize,
                font.Style, font.Unit,
                font.GdiCharSet, font.GdiVerticalFont);
        font.Dispose();
        return fontWithNewSize;
    }
}