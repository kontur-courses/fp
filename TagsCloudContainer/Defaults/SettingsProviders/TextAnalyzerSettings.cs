using Mono.Options;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class TextAnalyzerSettings : ICliSettingsProvider
{
    private static readonly char[] defaultSeparators = { ' ', ',', '.' };

    public char[] WordSeparators { get; private set; } = defaultSeparators;
    private const char separator = '/';

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            {
                "word-separators=",
                $"Set separators to separate words, separated by '{separator}'. Defaults to '{string.Join(separator, defaultSeparators)}'",
                v => WordSeparators = v.Split(separator).Cast<char>().ToArray()
            }
        };

        return options;
    }
}