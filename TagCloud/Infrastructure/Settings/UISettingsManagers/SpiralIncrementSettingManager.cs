using System;
using System.Collections.Generic;
using TagCloud.Infrastructure.Settings.SettingsProviders;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class SpiralIncrementSettingManager : IInputModifierManager
    {
        private readonly Func<ISpiralSettingsProvider> settingProvider;

        public SpiralIncrementSettingManager(Func<ISpiralSettingsProvider> settingProvider)
        {
            this.settingProvider = settingProvider;
            modifiers = new Dictionary<string, Func<Result<string>>>
            {
                {
                    "-", () =>
                    {
                        settingProvider().Increment--;
                        return Result.Ok("");
                    }
                },
                {
                    "+", () =>
                    {
                        settingProvider().Increment++;
                        return Result.Ok("");
                    }
                }
            };
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

        public IEnumerable<string> GetModifiers() => modifiers.Keys;

        private readonly Dictionary<string, Func<Result<string>>> modifiers;

        public Result<string> ApplyModifier(string modifier)
        {
            if (!modifiers.TryGetValue(modifier, out var action))
                return Result.Fail<string>("Incorrect modifier selected");
            action();
            return Get();
        }
    }
}