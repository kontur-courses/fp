using ResultOf;

namespace TagCloud.App
{
    public interface ISettingsProvider
    {
        Result<AppSettings> GetSettings();
    }
}
