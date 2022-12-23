namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public class TxtEncoder : IFileEncoder
{
    private const string FileType = "txt";

    public Result<string> GetText(string fileName)
    {
        return IsSuitableFileType(fileName)
            .Then(_ => Result.Of(() => File.ReadAllText(fileName))
                .RefineError("Can not read words from file"));
    }

    public Result<None> IsSuitableFileType(string fileName)
    {
        return fileName.EndsWith(FileType)
            ? Result.Ok()
            : Result.Fail<None>($"File is not suitable. Expected {FileType} filetype, " +
                                $"but was found {fileName.Split('.').LastOrDefault() ?? string.Empty}");
    }

    public string GetExpectedFileType()
    {
        return FileType;
    }
}