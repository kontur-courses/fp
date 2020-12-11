using System;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class SpiralIncrementSettingManager : ISettingsManager
    {
        private readonly Func<ISpiralSettingsProvider> settingProvider;

        public SpiralIncrementSettingManager(Func<ISpiralSettingsProvider> settingProvider)
        {
            this.settingProvider = settingProvider;
        }

        public string Title => "Spiral increment";
        public string Help => "Choose increment to change placing strategy";

        public Result<string> TrySet(string input)
        {
            if (!int.TryParse(input, out var number))
                return Result.Fail<string>("Incorrect input. Type integer number!");
            settingProvider().Increment = number;
            return Get();
        }

        public string Get()
        {
            return settingProvider().Increment.ToString();
        }
    }
}