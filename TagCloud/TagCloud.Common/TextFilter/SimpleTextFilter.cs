using System.Text.RegularExpressions;
using ResultOf;

namespace TagCloud.Common.TextFilter;

public class SimpleTextFilter : ITextFilter
{
    public Result<List<string>> FilterAllWords(IEnumerable<string> lines, int boringWordsLength)
    {
        var words = new List<string>();
        foreach (var line in lines)
        {
            words.AddRange(GetWords(line).Where(word => word.Length > boringWordsLength));
        }

        return words.Count == 0 ? Result.Fail<List<string>>("Filtered 0 words inside text") : words.AsResult();
    }

    private IEnumerable<string> GetWords(string input)
    {
        var matches = Regex.Matches(input, @"\b[\w']*\b");

        var words = from m in matches
            where !string.IsNullOrEmpty(m.Value)
            select TrimSuffix(m.Value).ToLower();

        return words.ToArray();
    }

    private string TrimSuffix(string word)
    {
        var apostropheLocation = word.IndexOf('\'');
        if (apostropheLocation != -1)
        {
            word = word[..apostropheLocation];
        }

        return word;
    }
}