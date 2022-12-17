using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCoreExtensions.WordsFilters.Settings;

namespace TagCloudCoreExtensions.WordsFilters;

public class WordsLengthFilter : IWordsFilter
{
    private readonly IWordsLengthFilterSettings _settings;

    public WordsLengthFilter(IWordsLengthFilterSettings settings)
    {
        _settings = settings;
    }

    public Result<IEnumerable<string>> FilterWords(IEnumerable<string> sourceWords) =>
        sourceWords
            .Where(word => word.Length >= _settings.MinWordLength && word.Length <= _settings.MaxWordLength)
            .AsResult();
}