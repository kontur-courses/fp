namespace TagsCloudContainer.FileProviders;

public interface IFileReader
{
    public Result<string> ReadFile(string filePath);
}