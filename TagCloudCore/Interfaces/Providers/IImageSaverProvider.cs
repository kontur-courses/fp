using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces.Providers;

public interface IImageSaverProvider
{
    IEnumerable<string> SupportedExtensions { get; }
    Result<IImageSaver> GetSaver();
}