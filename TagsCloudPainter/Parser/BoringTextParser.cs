using ResultLibrary;
using TagsCloudPainter.Settings;

namespace TagsCloudPainter.Parser;

public class BoringTextParser : ITextParser
{
    private static readonly string[] _separators = [" ", ". ", ", ", "; ", "-", "—", Environment.NewLine];
    private readonly ITextSettings textSettings;

    public BoringTextParser(ITextSettings textSettings)
    {
        this.textSettings = textSettings ?? throw new ArgumentNullException(nameof(textSettings));
    }

    public Result<List<string>> ParseText(string text)
    {
        var boringWords = GetBoringWords(textSettings.BoringText);
        var words = text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        var parssedText = boringWords
            .Then(boringWords => words.Select(word => word.ToLower()).Where(word => !boringWords.Contains(word)).ToList());

        return parssedText;
    }

    public Result<HashSet<string>> GetBoringWords(string text)
    {
        var words = Result.Of(() => text.Split(Environment.NewLine));
        var boringWords = words.Then((words) => words.Select(word => word.ToLower()).ToHashSet());

        return boringWords;
    }
}