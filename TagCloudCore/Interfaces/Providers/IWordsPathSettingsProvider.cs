using TagCloudCore.Interfaces.Settings;

namespace TagCloudCore.Interfaces.Providers;

public interface IWordsPathSettingsProvider
{
    IWordsPathSettings GetWordsPathSettings();
}