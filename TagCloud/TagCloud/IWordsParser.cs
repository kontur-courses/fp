using Result;

namespace TagCloud;

public interface IWordsParser
{
    public Result<List<string>> Parse(string? text);
}