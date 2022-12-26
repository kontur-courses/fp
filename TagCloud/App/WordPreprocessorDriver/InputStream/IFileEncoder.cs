namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public interface IFileEncoder
{
    Result<string> GetText(string fileName);

    bool IsSuitableFileType(string fileName);

    string GetExpectedFileType();
}