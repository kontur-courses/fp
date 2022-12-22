using GroupDocs.Parser.Options;
using GroupDocs.Parser;
using TagsCloudContainer.Interfaces;
using Result;

namespace TagsCloudContainer;

public class BudgetDocParser : IDocParser
{
    public Result<List<string>> ParseDoc(string filePath)
    {
        using var parser = new Parser(filePath);

        using var reader = parser.GetFormattedText(new FormattedTextOptions(FormattedTextMode.PlainText));
        return reader == null
            ? new Result<List<string>>(new Exception("Unsupported file format, or empty file"))
            : new Result<List<string>>(reader.ReadToEnd().Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                .ToList());
    }
}