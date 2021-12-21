using Autofac;
using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.SettingsProviders;

namespace TagsCloudContainer.Defaults;

public class TextAnalyzer : ITextAnalyzer
{
    private readonly ITextReader textReader;
    private readonly IWordNormalizer[] wordNormalizers;
    private readonly IWordFilter[] wordFilters;
    private readonly char[] wordSeparators;

    public TextAnalyzer(TextReaderSelector textReader, IWordNormalizer[] wordNormalizers, IWordFilter[] wordFilters, TextAnalyzerSettings settings) :
        this(textReader.GetReader(), wordNormalizers, wordFilters, settings.WordSeparators)
    {
    }

    protected TextAnalyzer(ITextReader textReader, IWordNormalizer[] wordNormalizers, IWordFilter[] wordFilters, char[] wordSeparators)
    {
        this.textReader = textReader;
        this.wordNormalizers = wordNormalizers;
        this.wordFilters = wordFilters;
        this.wordSeparators = wordSeparators;
    }

    public Result<ITextStats> AnalyzeText()
    {
        var stats = new TextStats();
        foreach (var line in textReader.ReadLines())
        {
            var words = line
                .Split(wordSeparators)
                .Where(x => !string.IsNullOrWhiteSpace(x) && x.Any(y => char.IsLetter(y)));
            foreach (var word in words)
            {
                var normalized = Normalize(word);
                var isValid = normalized.Then(IsValid)
                    .Then(isValid =>
                    {
                        if (isValid)
                            stats.UpdateWord(normalized.GetValueOrThrow());
                    });

                if (!isValid.IsSuccess)
                    return Result.Fail<ITextStats>(isValid.Error);
            }
        }

        return stats;
    }

    private Result<string> Normalize(string word)
    {
        var result = Result.Ok(word);
        foreach (var normalizer in wordNormalizers)
        {
            result = result.Then(normalizer.Normalize);
            if (!result.IsSuccess)
                return result;
        }

        return result;
    }

    private Result<bool> IsValid(string word)
    {
        foreach (var filter in wordFilters)
        {
            var result = filter.IsValid(word);
            if (!result.IsSuccess || !result.GetValueOrThrow())
                return result;
        }

        return true;
    }
}
