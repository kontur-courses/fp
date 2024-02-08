using System.Drawing;

namespace TagsCloud.Layouters;

public static class ILayouterExtensions
{
    public static Size GetSizeFromFont(this ILayouter layouter,string word,Font font)
    {
        using var g = Graphics.FromImage(new Bitmap(1, 1));
        var sizeF = g.MeasureString(word, font);
        return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
    }
}