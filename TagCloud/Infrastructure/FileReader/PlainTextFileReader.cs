using TagCloud.Infrastructure.Monad;

namespace TagCloud.Infrastructure.FileReader;

public class PlainTextFileReader : IFileReader
{
    public Result<IEnumerable<string>> GetLines(string inputPath)
    {
        return !File.Exists(inputPath) 
            ? Result.Fail<IEnumerable<string>>($"The file does not exist {inputPath}") 
            : Result.Ok(File.ReadLines(inputPath));
    }

    public virtual IEnumerable<string> GetSupportedExtensions()
    {
        yield return ".txt";
    }
}