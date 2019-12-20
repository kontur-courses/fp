using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Settings_Providing
{
    public interface ISettingsProvider
    {
        Result<Settings> GetSettings();
    }
}