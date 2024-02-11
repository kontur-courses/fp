using Newtonsoft.Json;
using ResultOf;
using TagsCloudContainer.SettingsClasses;

namespace WinFormsApp.SettingsManager
{
    public static class SettingsManager
    {
        private const string settingsFile = "settings.json";
        public static void SaveSettings(AppSettings settings, string filePath = settingsFile)
        {
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings));
        }

        public static AppSettings LoadSettings(string filePath = settingsFile)
        {
            AppSettings settings = new();
            settings.DrawingSettings = new CloudDrawingSettings();

            if (File.Exists(filePath))
            {
                var tryReadSettings = ReadFromFile(filePath);
                if (!tryReadSettings.IsSuccess)
                {
                    ErrorMessageBox.ShowError(tryReadSettings.Error);
                    settings = new AppSettings();
                    settings.DrawingSettings = new CloudDrawingSettings();
                    SaveSettings(settings);
                }
                settings = tryReadSettings.GetValueOrDefault();

                if (settings.DrawingSettings == null)
                {
                    ErrorMessageBox.ShowError("Drawing setting not found. Default is loaded.");
                    settings.DrawingSettings = new CloudDrawingSettings();
                    SaveSettings(settings);
                }
            }

            return settings;
        }

        private static Result<AppSettings> ReadFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return Result<AppSettings>.Ok(JsonConvert.DeserializeObject<AppSettings>(json));
            }
            catch (JsonException ex)
            {
                return Result<AppSettings>.Fail($"Settings file is damaged: {ex.Message}. Default settings is loaded.");
            }
            catch (Exception ex)
            {
                return Result<AppSettings>.Fail($"Failed to read file: {ex.Message}");
            }
        }
    }
}