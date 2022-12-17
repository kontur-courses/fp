using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCore.Domain.Providers;

public class ImageSaverProvider : IImageSaverProvider
{
    private readonly Dictionary<string, IImageSaver> _imageSavers;
    private readonly IImagePathSettingsProvider _pathSettingsProvider;

    public ImageSaverProvider(
        IEnumerable<IImageSaver> wordsFileReaders,
        IImagePathSettingsProvider pathSettingsProvider
    )
    {
        _imageSavers = wordsFileReaders.ToDictionary(saver => saver.SupportedExtension);
        _pathSettingsProvider = pathSettingsProvider;
    }

    public IEnumerable<string> SupportedExtensions => _imageSavers.Keys;

    public Result<IImageSaver> GetSaver()
    {
        var imageExtension = Path.GetExtension(_pathSettingsProvider.GetImagePathSettings().ImagePath);
        return _imageSavers.TryGetValue(imageExtension, out var result)
            ? result.AsResult()
            : Result.Fail<IImageSaver>($"No saver for extension: {imageExtension}");
    }
}