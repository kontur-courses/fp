using System;
using System.Drawing;
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

        public Result<string> TrySet(string name)
        {
            var fontFamily = new FontFamily(name);
            if (fontFamily.Name == name)
                settingProvider().FontFamily = fontFamily;
            else
                return Result.Fail<string>($"FontFamily {name} was not found!");
            return Get();
        }

        public string Get()
        {
            return settingProvider().FontFamily.Name;
        }
    }
}