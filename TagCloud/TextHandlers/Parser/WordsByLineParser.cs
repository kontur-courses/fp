using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagCloud.TextHandlers.Parser;

public class WordsByLineParser : ITextParser
{
    public Result<IEnumerable<string>> GetWords(string path)
    {
        return File.ReadAllLines(path)
            .AsEnumerable()
            .AsResult()
            .ReplaceError(_ => $"Cannot read from file {path}");
    }
}