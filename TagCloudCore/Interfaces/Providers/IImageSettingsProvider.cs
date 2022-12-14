using TagCloudCore.Domain.Settings;

namespace TagCloudCore.Interfaces.Providers;

public interface IImageSettingsProvider
{
    ImageSettings GetImageSettings();
}