using System.IO;
using Newtonsoft.Json;
using TagsCloudCreating.Infrastructure;

namespace TagsCloudVisualization.Infrastructure.Common
{
    public static class SettingsSerializer
    {
        private const string PathToSettings = "appsettings.json";
        private static SettingsManager SettingsManager { get; set; }

        public static SettingsManager Deserialize()
        {
            var settingsManager = ReadAllText(PathToSettings)
                .Then(JsonConvert.DeserializeObject<SettingsManager>)
                .OnFail(_ => ShowMessage());

            SettingsManager = settingsManager.IsSuccess ? settingsManager.GetValueOrThrow() : new SettingsManager();
            return SettingsManager;
        }

        private static void ShowMessage() =>
            InformationMessageHelper.ShowExceptionMessage($"{PathToSettings} not found! Default settings applied.");

        private static Result<string> ReadAllText(string path) =>
            File.Exists(path)
                ? File.ReadAllText(PathToSettings)
                : Result.Fail<string>("File is not found!");

        public static void Serialize() =>
            File.WriteAllText(PathToSettings, JsonConvert.SerializeObject(SettingsManager));
    }
}