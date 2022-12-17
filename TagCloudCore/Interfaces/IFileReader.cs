using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Interfaces;

public interface IFileReader
{
    public string SupportedExtension { get; }
    Result<string> ReadFile();
}