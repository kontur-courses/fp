namespace TagsCloudVisualization.ImageSettings;

public interface IImageSettingsProvider
{
    Result<ImageSettings> GetSettings();
}