using ResultOf;

namespace TagCloud.FileReader;

public class TxtReader : IFileReader
{
    public IEnumerable<string> GetAvailableExtensions() => new List<string>() { "txt" };

    public Result<IEnumerable<string>> ReadLines(string inputPath)
    {
        var fileExistsResult = FileReaderUtils.FileExists(inputPath);

        return fileExistsResult.IsSuccess
            ? Result.Ok(File.ReadLines(inputPath))
            : Result.Fail<IEnumerable<string>>(fileExistsResult.Error);
    }
}