using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public interface IImageSettingsProvider
    {
        Result<None> SetImageSettings(ImageSettings imageSettings);

        ImageSettings ImageSettings { get; }
    }
}