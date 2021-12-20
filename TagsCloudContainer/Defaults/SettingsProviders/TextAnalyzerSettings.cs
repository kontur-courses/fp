using Mono.Options;
using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class TextAnalyzerSettings : ICliSettingsProvider
{
    private static readonly char[] defaultSeparators = { ' ', ',', '.' };

    public char[] WordSeparators { get; private set; } = defaultSeparators;
    private const char separator = '/';

    public Result State { get; private set; } = Result.Ok();
    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            {
                "word-separators=",
                $"Set separators to separate words, separated by '{separator}'. Defaults to '{string.Join(separator, defaultSeparators)}'",
                v => State = Result.Of(() => v.Split(separator).Cast<char>().ToArray()).Then( s => WordSeparators = s)
            }
        };

        return options;
    }
}