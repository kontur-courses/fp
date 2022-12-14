using TagCloudCore.Domain.Settings;
using TagCloudCore.Infrastructure.Settings;
using TagCloudCore.Interfaces.Providers;
using TagCloudCore.Interfaces.Settings;

namespace TagCloudCore.Domain.Providers;

public class AppSettingsProvider : IWordsPathSettingsProvider, IImagePathSettingsProvider, IImageSettingsProvider
{
    private readonly SettingsManager _settingsManager;

    public AppSettingsProvider(SettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public IWordsPathSettings GetWordsPathSettings() =>
        _settingsManager.AppSettings.Value;

    public IImagePathSettings GetImagePathSettings() =>
        _settingsManager.AppSettings.Value;

    public ImageSettings GetImageSettings() =>
        _settingsManager.AppSettings.Value.ImageSettings;
}