using System.Drawing;
using TagsCloud.Result;

namespace TagsCloud.WordFontCalculators;

public interface IWordFontCalculator
{
    public Result<Font> GetWordFont(string word, int count);
}