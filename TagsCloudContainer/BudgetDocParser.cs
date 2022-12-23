using GroupDocs.Parser.Options;
using GroupDocs.Parser;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainer;

public class BudgetDocParser : IDocParser
{
    public Result<List<string>> ParseDoc(string filePath)
    {
        using var parser = new Parser(filePath);

        using var reader = parser.GetFormattedText(new FormattedTextOptions(FormattedTextMode.PlainText));
        return reader is null
            ? Result.Fail<List<string>>("Unsupported file format, or empty file")
            : reader.ReadToEnd().Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                .ToList().AsResult();
    }
}