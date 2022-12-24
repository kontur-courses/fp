using System.Drawing;
using TagCloudResult.Words;

namespace TagCloudResult.Extensions;

public class WordMeasurer : IDisposable
{
    private readonly Bitmap _bitmap;
    private readonly Graphics _graphics;

    public WordMeasurer()
    {
        _bitmap = new Bitmap(1, 1);
        _graphics = Graphics.FromImage(_bitmap);
    }

    public void Dispose()
    {
        _graphics.Dispose();
        _bitmap.Dispose();
    }

    public Size MeasureWord(Word word, Font font)
    {
        var size = _graphics.MeasureString(word.Value, font);
        if (size.Width < 1) size.Width = 1;
        if (size.Height < 1) size.Height = 1;
        return size.ToSize();
    }
}