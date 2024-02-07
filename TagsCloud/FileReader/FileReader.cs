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
                : Result.Fail<IEnumerable<string>>(
                    $"К сожалению, эта программа поддерживает только файлы с расширениями txt, doc и docx.\n "
                    + $"Попробуйте сконвертировать ваш файл с расширением {Path.GetExtension(filePath).Trim('.')} в один из указанных типов.")
            : Result.Fail<IEnumerable<string>>($"Файл по пути '{filePath}' не найден");
    }
}