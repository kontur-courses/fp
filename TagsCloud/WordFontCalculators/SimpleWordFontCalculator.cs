using System.Drawing;
using System.Drawing.Text;
using TagsCloud.ConsoleCommands;

namespace TagsCloud.WordFontCalculators;

public class SimpleWordFontCalculator : IWordFontCalculator
{
    private readonly string _font;

    public SimpleWordFontCalculator(Options options)
    {
        this._font = options.TagsFont;
    }

    public Result<Dictionary<string, Font>> GetWordsFont(Dictionary<string, int> wordsDictionary)
    {
        if (!wordsDictionary.Any())
            return Result.Fail<Dictionary<string, Font>>("Dictionary contains no elements");

        if (wordsDictionary.Any(w => w.Value == 0))
            return Result.Fail<Dictionary<string, Font>>("Dictionary contains key with value 0");

        var fonts = new InstalledFontCollection();
        if (fonts.Families.All(f => f.Name != _font))
        {
            return Result.Fail<Dictionary<string, Font>>($"There is no font with this name: {_font}");
        }
        var result = wordsDictionary.ToDictionary(entry => entry.Key, 
            entry => new Font(_font, entry.Value));
        return result;
    }
}