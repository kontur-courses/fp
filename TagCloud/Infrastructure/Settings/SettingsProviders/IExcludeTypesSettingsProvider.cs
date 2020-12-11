using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IExcludeTypesSettingsProvider
    {
        public WordType[] ExcludedTypes { get; }
    }
}