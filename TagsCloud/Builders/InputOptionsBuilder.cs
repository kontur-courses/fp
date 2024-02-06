using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloud.Results;

namespace TagsCloud.Builders;

public class InputOptionsBuilder
{
    private readonly HashSet<string> supportedLanguageParts = new(StringComparer.OrdinalIgnoreCase)
    {
        "A", "ADV", "ADVPRO",
        "ANUM", "APRO", "COM",
        "CONJ", "INTJ", "NUM",
        "PART", "PR", "S",
        "SPRO", "V"
    };

    private HashSet<string> excludedWords;
    private HashSet<string> languageParts;
    private bool onlyRussian;
    private bool toInfinitive;
    private CaseType wordsCase;

    public Result<InputOptionsBuilder> SetWordsCase(CaseType caseType)
    {
        wordsCase = caseType;
        return this;
    }

    public Result<InputOptionsBuilder> SetCastPolitics(bool caseToInfinitive)
    {
        toInfinitive = caseToInfinitive;
        return this;
    }

    public Result<InputOptionsBuilder> SetLanguagePolitics(bool russian)
    {
        onlyRussian = russian;
        return this;
    }

    public Result<InputOptionsBuilder> SetLanguageParts(IEnumerable<string> parts)
    {
        languageParts = parts.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var unknownParts = languageParts
                           .Where(part => !supportedLanguageParts.Contains(part))
                           .ToList();

        if (unknownParts.Count == 0)
            return this;

        var candidates = string.Join(", ", supportedLanguageParts);
        return ResultExtensions.Fail<InputOptionsBuilder>(
            $"Unknown language parts: {string.Join(", ", unknownParts)}. Candidates are: {candidates}");
    }

    public Result<InputOptionsBuilder> SetExcludedWords(IEnumerable<string> excluded)
    {
        excludedWords = excluded.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return this;
    }

    public Result<IInputProcessorOptions> BuildOptions()
    {
        return new InputProcessorOptions
        {
            ToInfinitive = toInfinitive,
            OnlyRussian = onlyRussian,
            LanguageParts = languageParts,
            WordsCase = wordsCase,
            ExcludedWords = excludedWords
        };
    }
}