using ResultLibrary;

namespace TagsCloudPainter.Parser;

public interface ITextParser
{
    Result<List<string>> ParseText(string text);
}