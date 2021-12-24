using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Templates;

internal class Template : ITemplate
{
    public Size ImageSize { get; init; }
    public Color BackgroundColor { get; set; }
    private readonly List<WordParameter> words;
    private RectangleF cloud;

    public Template()
    {
        words = new List<WordParameter>();
    }

    public Template(IEnumerable<WordParameter> words)
    {
        this.words = words.ToList();
    }

    public bool TryAdd(WordParameter wordParameter)
    {
        if (!TryIncreaseSize(wordParameter.WordRectangleF))
            return false;
        words.Add(wordParameter);
        return true;
    }

    public IEnumerable<WordParameter> GetWordParameters()
    {
        return words;
    }

    private bool TryIncreaseSize(RectangleF rectangle)
    {
        var union = RectangleF.Union(cloud, rectangle);
        if (union.Width > ImageSize.Width || union.Height > ImageSize.Height)
            return false;

        cloud = union;
        return true;
    }
}