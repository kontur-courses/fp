namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public class TxtEncoder : IFileEncoder
{
    private const string FileType = "txt";

    public Result<string> GetText(string fileName)
    {
        var result = IsSuitableFileType(fileName)
            ? Result.Of(() => File.ReadAllText(fileName))
            : Result.Fail<string>("Filetype is not suitable");
        return result.RefineError("Can not read words from file");
    }

    public bool IsSuitableFileType(string fileName)
    {
        return fileName.EndsWith(FileType);
    }

    public string GetExpectedFileType()
    {
        return FileType;
    }
}