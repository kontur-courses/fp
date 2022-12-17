using System.Text.RegularExpressions;
using MyStemWrapper;
using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;

namespace TagCloudCoreExtensions;

public class MyStemWordsNormalizer : IWordsNormalizer
{
    private readonly MyStemWordsGrammarInfoParser _grammarInfoParser;

    public MyStemWordsNormalizer(MyStemWordsGrammarInfoParser grammarInfoParser)
    {
        _grammarInfoParser = grammarInfoParser;
    }

    public Result<IEnumerable<string>> GetWordsOriginalForm(string sourceText)
    {
        var sourceWords = Regex.Split(sourceText.ToLower(), @"\W+")
            .Where(word => !string.IsNullOrWhiteSpace(word));

        return Result.Of(() =>
            _grammarInfoParser.Parse(sourceWords)
                .Select(grammarInfo => grammarInfo.OriginalForm)
        );
    }
}