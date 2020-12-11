using System;
using System.Drawing;
using ResultOf;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class FontSettingManager : ISettingsManager
    {
        private readonly Func<IFontSettingProvider> settingProvider;

        public FontSettingManager(Func<IFontSettingProvider> settingProvider)
        {
            this.settingProvider = settingProvider;
        }

        public string Title => "Font";
        public string Help => "Choose font family name to write tags";

        public Result<string> TrySet(string path)
        {
            var fontFamily = new FontFamily(path);
            if (fontFamily.Name == path)
                settingProvider().FontFamily = fontFamily;
            else
                return Result.Fail<string>($"FontFamily {path} was not found!");
            return Get();
        }

        public string Get()
        {
            return settingProvider().FontFamily.Name;
        }
    }
}