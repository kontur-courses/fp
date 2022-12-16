using Result;

namespace TagCloud;

public class WordsParser : IWordsParser
{
    public Result<List<string>> Parse(string? text)
    {
        if (text is null)
            return new Result<List<string>>(new List<string>(), "Can't parse null string");

        var parsedWords = text
            .Split(new[] { "\n", " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        return new Result<List<string>>(parsedWords);
    }
}