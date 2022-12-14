namespace TagCloudCore.Interfaces.Providers;

public interface IImageSaverProvider
{
    IEnumerable<string> SupportedExtensions { get; }
    IImageSaver GetSaver();
}