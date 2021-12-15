using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public class PlainTextFileReader : IFileReader
{
    private static readonly IReadOnlySet<string> SupportedExtensions = new HashSet<string> { ".txt"};

    public Result<IEnumerable<string>> GetLines(string inputPath)
    {
        return !File.Exists(inputPath) 
            ? Result.Fail<IEnumerable<string>>($"The file does not exist {inputPath}") 
            : Result.Ok(File.ReadLines(inputPath));
    }

    public IEnumerable<string> GetSupportedExtensions()
    {
        return SupportedExtensions;
    }
}