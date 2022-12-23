using System.Drawing;
using TagCloudResult.Words;

namespace TagCloudResult.Extensions;

public static class WordExtensions
{
    private static readonly Bitmap _bitmap = new(1, 1);
    private static readonly Graphics _graphics = Graphics.FromImage(_bitmap);

    public static Size MeasureWord(this Word word, Font font)
    {
        var size = _graphics.MeasureString(word.Value, font);
        if (size.Width < 1) size.Width = 1;
        if (size.Height < 1) size.Height = 1;
        return size.ToSize();
    }
}