using System.Drawing;
using System.Drawing.Text;
using TagsCloud.ConsoleOptions;
using TagsCloud.Options;

namespace TagsCloud.WordFontCalculators;

public class SimpleWordFontCalculator : IWordFontCalculator
{
    private readonly string _font;

    public SimpleWordFontCalculator(LayouterOptions options)
    {
        _font = options.FontName;
    }

    public Result<Dictionary<string, Font>> GetWordsFont(Dictionary<string, int> wordsDictionary)
    {
        if (!wordsDictionary.Any())
            return Result.Fail<Dictionary<string, Font>>("Dictionary contains no elements");

        if (wordsDictionary.Any(w => w.Value == 0))
            return Result.Fail<Dictionary<string, Font>>("Dictionary contains key with value 0");
        
        var result = wordsDictionary.ToDictionary(entry => entry.Key, 
            entry => new Font(_font, entry.Value));
        return result;
    }
}