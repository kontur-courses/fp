using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Extensions;

namespace TagCloud.Templates;

public class Template : ITemplate
{
    private readonly List<WordParameter> words = new();
    public Size ImageSize { get; init; }
    public Color BackgroundColor { get; set; }
    private SizeF cloudSize = Size.Empty;

    public Template()
    {
    }

    public Template(IEnumerable<WordParameter> words)
    {
        this.words = words.ToList();
    }

    public bool TryAdd(WordParameter wordParameter)
    {
        if (!TryIncreaseSize(wordParameter.WordRectangleF.Size))
            return false;
        words.Add(wordParameter);
        return true;
    }

    public IEnumerable<WordParameter> GetWordParameters()
    {
        return words;
    }

    private bool TryIncreaseSize(SizeF size)
    {
        var increasedSize = cloudSize.Combine(size);
        if (increasedSize.Width > ImageSize.Width || increasedSize.Height > ImageSize.Height)
            return false;

        cloudSize = increasedSize;
        return true;
    }
}