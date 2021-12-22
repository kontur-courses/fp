using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagCloud.TextHandlers.Parser;

public class WordsFromTextParser : ITextParser
{
    public Result<IEnumerable<string>> GetWords(string path)
    {
        return File.ReadAllText(path)
            .AsResult()
            .Then(i => Regex.Matches(i, @"\b\w*\b"))
            .Then(m => m.Cast<Match>())
            .Then(e => e.Where(m => !string.IsNullOrEmpty(m.Value)))
            .Then(e => e.Select(m => m.Value))
            .ReplaceError(_ => $"Error on reading {path}");
    }
}