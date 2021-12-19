namespace TagsCloudContainer.FileReaders
{
    public interface IFileReaderFactory
    {
        Result<IFileReader> GetProperFileReader(string path);
    }
}