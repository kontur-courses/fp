using System.Drawing;
using System.Drawing.Text;
using TagsCloud.ConsoleCommands;
using TagsCloud.Result;

namespace TagsCloud.WordFontCalculators;

public class SimpleWordFontCalculator : IWordFontCalculator
{
    private readonly string font;

    public SimpleWordFontCalculator(Options options)
    {
        this.font = options.TagsFont;
    }

    public Result<Font> GetWordFont(string word, int count)
    {
        if (string.IsNullOrEmpty(word))
            return  Result.Result.Fail<Font>("Word is null or empty");

        if (count <= 0)
            return Result.Result.Fail<Font>("Count is less than or equal to 0");
        
        var fonts = new InstalledFontCollection();
        if (fonts.Families.Any(f=>f.Name==font))
        {
            return Result.Result.Ok<Font>( new Font(font, count));
        }
        
        return Result.Result.Fail<Font>($"There is no font with this name: {font}");

    }
}