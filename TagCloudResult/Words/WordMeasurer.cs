using System.Drawing;
using TagCloudResult.Words;

namespace TagCloudResult.Extensions;

public class WordMeasurer : IDisposable
{
    private readonly Graphics _graphics;

    public WordMeasurer()
    {
        _graphics = Graphics.FromHwnd(IntPtr.Zero);
    }

    public void Dispose()
    {
        _graphics.Dispose();
    }

    public Size MeasureWord(Word word, Font font)
    {
        var size = _graphics.MeasureString(word.Value, font);
        if (size.Width < 1) size.Width = 1;
        if (size.Height < 1) size.Height = 1;
        return size.ToSize();
    }
}