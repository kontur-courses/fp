using TagCloudCore.Domain.Settings;
using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Infrastructure.Settings;

public class SettingsManager
{
    private const string SettingsFilename = "app.settings";
    private readonly IObjectSerializer _serializer;
    private readonly IBlobStorage _storage;

    public SettingsManager(IObjectSerializer serializer, IBlobStorage storage)
    {
        _serializer = serializer;
        _storage = storage;
    }

    public Result<AppSettings> LoadSettingsFromFile() =>
        _storage.Get(SettingsFilename)
            .Then(bytes => _serializer.Deserialize<AppSettings>(bytes))
            .RefineError("Can't get settings from file");

    public AppSettings CreateDefaultSettings() =>
        new()
        {
            ImagePath = ".",
            WordsPath = ".",
            ImageSettings = new ImageSettings
            {
                Width = 800,
                Height = 600
            }
        };

    public Result<None> Save(AppSettings settings) =>
        _serializer.Serialize(settings)
            .Then(bytes => _storage.Set(SettingsFilename, bytes))
            .RefineError("Can't save settings to file");
}