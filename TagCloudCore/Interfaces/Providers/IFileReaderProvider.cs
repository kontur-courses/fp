using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces.Providers;

public interface IFileReaderProvider
{
    IEnumerable<string> SupportedExtensions { get; }
    Result<IFileReader> GetReader();
}