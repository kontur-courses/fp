using ResultOf;

namespace TagCloud.FileReader;

public static class FileReaderUtils
{
    public static Result<bool> FileExists(string inputPath)
    {
        return !File.Exists(inputPath)
            ? Result.Fail<bool>($"Input file {inputPath} doesn't exist")
            : Result.Ok(true);
    }
}