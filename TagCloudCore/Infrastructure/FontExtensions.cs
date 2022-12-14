using System.Drawing;

namespace TagCloudCore.Infrastructure;

public static class FontExtensions
{
    public static Font WithSize(this Font font, int newSize) =>
        new(font.FontFamily, newSize, font.Style);
}