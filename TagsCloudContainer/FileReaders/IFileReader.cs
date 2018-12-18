namespace TagsCloudContainer.FileReaders
{
    public interface IFileReader
    {
        Result<string> Read();
    }
}