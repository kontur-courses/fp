namespace TagsCloudGenerator.FileReaders
{
    public interface IReaderFactory
    {
        Result<IFileReader> GetReaderFor(string extension);
    }
}