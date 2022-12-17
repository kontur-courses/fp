using TagCloudCore.Domain.Settings;
using TagCloudCore.Infrastructure.Settings;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;
using TagCloudCore.Interfaces.Settings;

namespace TagCloudCore.Domain.Providers;

public class AppSettingsProvider : IWordsPathSettingsProvider, IImagePathSettingsProvider, IImageSettingsProvider
{
    private readonly SettingsManager _settingsManager;
    private readonly IErrorHandler _errorHandler;
    private AppSettings? _appSettings;

    public AppSettingsProvider(SettingsManager settingsManager, IErrorHandler errorHandler)
    {
        _settingsManager = settingsManager;
        _errorHandler = errorHandler;
    }

    public IWordsPathSettings GetWordsPathSettings() =>
        GetAppSettings();

    public IImagePathSettings GetImagePathSettings() =>
        GetAppSettings();

    public ImageSettings GetImageSettings() =>
        GetAppSettings().ImageSettings;

    public AppSettings GetAppSettings()
    {
        if (_appSettings is not null)
            return _appSettings;

        var settingsResult = _settingsManager.LoadSettingsFromFile();
        if (settingsResult.IsSuccess)
            return _appSettings ??= settingsResult.Value;
        _errorHandler.HandleError(settingsResult.Error);
        return _appSettings ??= _settingsManager.CreateDefaultSettings();
    }
}