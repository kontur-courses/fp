namespace TagsCloud;

public class FileReader
{
    private readonly Dictionary<string, IParser> parsers;

    public FileReader(IEnumerable<IParser> parsers)
    {
        this.parsers = parsers.ToDictionary(parser => parser.FileType);
    }

    public Result<IEnumerable<string>> GetWords(string filePath)
    {
        return File.Exists(filePath)
            ? parsers.TryGetValue(Path.GetExtension(filePath).Trim('.'), out var parser)
                ? Result.Ok<IEnumerable<string>>(parser.GetWordList(filePath))
                : Result.Fail<IEnumerable<string>>($"not found parser for {Path.GetExtension(filePath).Trim('.')}")
            : Result.Fail<IEnumerable<string>>($"Error: File not found at the specified path '{filePath}'.");
    }
}