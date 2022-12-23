namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public interface IFileEncoder
{
    Result<string> GetText(string fileName);
        
    Result<None> IsSuitableFileType(string fileName);
        
    string GetExpectedFileType();
}