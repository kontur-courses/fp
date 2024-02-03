using ResultOf;

namespace TagCloud.FileReader;

public class TxtReader : IFileReader
{
    public IEnumerable<string> GetAvailableExtensions() => new List<string>() { "txt" };

    public Result<IEnumerable<string>> ReadLines(string inputPath)
    {
        return FileExists(inputPath, out var error)
            ? Result.Ok(File.ReadLines(inputPath))
            : Result.Fail<IEnumerable<string>>(error);
    }

    private bool FileExists(string inputPath, out string error)
    {
        if (!File.Exists(inputPath))
        {
            error = $"Input file {inputPath} doesn't exist";
            return false;
        }

        error = string.Empty;
        return true;
    }
}