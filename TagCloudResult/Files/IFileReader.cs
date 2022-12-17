namespace TagCloudResult.Files;

public interface IFileReader
{
    public string Extension { get; }
    public string ReadAll(string filename);
}