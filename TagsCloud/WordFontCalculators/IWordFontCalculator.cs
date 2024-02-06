using System.Drawing;
namespace TagsCloud.WordFontCalculators;

public interface IWordFontCalculator
{
    public Result<Dictionary<string,Font>> GetWordsFont(Dictionary<string,int> wordDict);
}