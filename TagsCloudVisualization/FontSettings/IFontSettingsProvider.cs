namespace TagsCloudVisualization.FontSettings;

public interface IFontSettingsProvider
{
    Result<FontSettings> GetSettings();
}