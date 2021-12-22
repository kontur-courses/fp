using System.Collections.Generic;
using System.Linq;
using TagCloud.Extensions;

namespace TagCloud.Templates;

public class FontSizeByCountCalculator : IFontSizeCalculator
{
    private readonly float maxSize;
    private readonly float minSize;

    public FontSizeByCountCalculator(float minSize, float maxSize)
    {
        this.maxSize = maxSize;
        this.minSize = minSize;
    }

    public Dictionary<string, float> GetFontSizes(IEnumerable<string> words)
    {
        var wordToCount = words.GetCountByItems();
        var maxCount = wordToCount.Max(k => k.Value);
        var minCount = wordToCount.Min(k => k.Value);
        var wordToSize = new Dictionary<string, float>();
        var divider = (maxCount - minCount) / (maxSize - minSize);
        if (divider == 0)
        {
            divider = 1;
        }

        foreach (var (word, wordCount) in wordToCount)
            wordToSize[word] = (wordCount - minCount) / divider + minSize;
        return wordToSize;
    }
}