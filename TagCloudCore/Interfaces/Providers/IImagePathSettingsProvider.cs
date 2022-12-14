using TagCloudCore.Interfaces.Settings;

namespace TagCloudCore.Interfaces.Providers;

public interface IImagePathSettingsProvider
{
    IImagePathSettings GetImagePathSettings();
}