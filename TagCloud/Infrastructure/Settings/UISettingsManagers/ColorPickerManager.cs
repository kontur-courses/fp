using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ColorPickerSettingsManager : IMultiOptionsManager
    {
        private readonly Regex regex;
        private readonly Func<IColorPickerSettingProvider> colorSettings;

        public ColorPickerSettingsManager(Func<IColorPickerSettingProvider> colorSettings)
        {
            this.colorSettings = colorSettings;
            regex = new Regex(@"\s*(?<type>.*?)\s*=\s*(?<color>.*?)\s*(;|$)");
        }

        private static IEnumerable<WordType> SupportedWordTypes => 
            Enum.GetValues(typeof(WordType)).Cast<WordType>();

        private static IEnumerable<Color> SupportedColors =>
            Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(Color.FromKnownColor);
        public string Title => "Color";
        public string Help => $"Choose color for word types {string.Join(" ", SupportedWordTypes)}";

        public Result<string> TrySet(string input)
        {
            var inputDictionary = regex.Matches(input)
                .OfType<Match>()
                .ToDictionary(m => m.Groups["type"].Value, m => m.Groups["color"].Value);

            var unknownColors = new List<string>();
            var unknownWordTypes = new List<string>();
            foreach (var typeName in inputDictionary.Keys)
            {
                if (Enum.TryParse<WordType>(typeName, out var type)
                    && SupportedWordTypes.Contains(type))
                {
                    var colorName = inputDictionary[typeName];
                    if (Enum.TryParse<KnownColor>(colorName, out var color)
                        && SupportedColors.Contains(Color.FromKnownColor(color)))
                        colorSettings().ColorMap[type] = Color.FromKnownColor(color);
                    else
                        unknownColors.Add(colorName);
                }
                else
                {
                    unknownWordTypes.Add(typeName);
                }
            }

            var notSuppliedWordTypes = SupportedWordTypes.Where(type => !inputDictionary.ContainsKey(type.ToString()));
            var sb = new StringBuilder();
            sb.Append("Values set.");
            sb.Append("WordTypes that was not supplied, and will be set automatically:");
            foreach (var wordType in notSuppliedWordTypes)
                sb.Append("\t").Append(wordType);
            sb.Append("Failed:");
            sb.Append("Unknown Colors:");
            foreach (var unknownColor in unknownColors)
                sb.Append("\t").Append(unknownColor);
            sb.Append("Unknown wordTypes:");
            foreach (var unknownWordType in unknownWordTypes)
                sb.Append("\t").Append(unknownWordType);
            return sb.ToString();
        }

        public string Get()
        {
            return string.Join("; ", colorSettings().ColorMap.Select(pair => $"{pair.Key} = {pair.Value.Name}"));
        }

        public Dictionary<string, IEnumerable<string>> GetOptions()
        {
            return SupportedWordTypes
                .ToDictionary(
                    type => type.ToString(), 
                    type => SupportedColors.Select(color => color.ToString()));
        }
    }
}