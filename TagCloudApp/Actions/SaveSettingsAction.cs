using TagCloudApp.Domain;
using TagCloudApp.Infrastructure;
using TagCloudCore.Domain.Providers;
using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Infrastructure.Settings;
using TagCloudCore.Interfaces;

namespace TagCloudApp.Actions;

public class SaveSettingsAction : IUiAction
{
    private readonly SettingsManager _settingsManager;
    private readonly AppSettingsProvider _appSettingsProvider;
    private readonly IErrorHandler _errorHandler;

    public SaveSettingsAction(SettingsManager settingsManager, AppSettingsProvider appSettingsProvider,
        IErrorHandler errorHandler)
    {
        _settingsManager = settingsManager;
        _appSettingsProvider = appSettingsProvider;
        _errorHandler = errorHandler;
    }

    public MenuCategory Category => MenuCategory.File;
    public string Name => "Save settings";
    public string Description => "Save settings";

    public void Perform()
    {
        _settingsManager.Save(_appSettingsProvider.GetAppSettings())
            .OnFail(_errorHandler.HandleError);
    }
}