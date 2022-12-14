namespace TagCloudCore.Interfaces.Providers;

public interface IFileReaderProvider
{
    IEnumerable<string> SupportedExtensions { get; }
    IFileReader GetReader();
}