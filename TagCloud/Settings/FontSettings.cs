using System.Drawing;
using Newtonsoft.Json;

namespace TagCloud.Settings
{
    public class FontSettings
    {
        private readonly SettingsLoader settingsLoader = new SettingsLoader();
        public FontFamily FontFamily { get; set; } = new FontFamily("Times New Roman");
        public int MinFontSize { get; set; } = 10;
        public int MaxFontSize { get; set; } = 50;

        public void UpdateSettings()
        {
            var settings = settingsLoader.Settings["FontSettings"];
            var fontSettings = JsonConvert.DeserializeObject<FontSettings>(settings);
            FontFamily = fontSettings.FontFamily;
            MinFontSize = fontSettings.MinFontSize;
            MaxFontSize = fontSettings.MaxFontSize;
        }
    }
}