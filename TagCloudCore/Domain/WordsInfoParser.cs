using TagCloudCore.Infrastructure;
using TagCloudCore.Infrastructure.Results;
using TagCloudCore.Interfaces;
using TagCloudCore.Interfaces.Providers;

namespace TagCloudCore.Domain;

public class WordsInfoParser : IWordsInfoParser
{
    private readonly IFileReaderProvider _fileReaderProvider;
    private readonly IWordsNormalizer _wordsNormalizer;
    private readonly ICollection<IWordsFilter> _wordsFilters;

    public WordsInfoParser(
        IFileReaderProvider fileReaderProvider,
        IWordsNormalizer wordsNormalizer,
        ICollection<IWordsFilter> wordsFilters
    )
    {
        _fileReaderProvider = fileReaderProvider;
        _wordsNormalizer = wordsNormalizer;
        _wordsFilters = wordsFilters;
    }

    public Result<IEnumerable<WordInfo>> GetWordsInfo() =>
        _fileReaderProvider.GetReader()
            .Then(reader => reader.ReadFile())
            .Then(text => _wordsNormalizer.GetWordsOriginalForm(text))
            .Then(FilterWords)
            .Then(filtered => filtered
                .GroupBy(w => w)
                .Select(group => new WordInfo(group.Key, group.Count()))
            );

    private Result<IEnumerable<string>> FilterWords(IEnumerable<string> sourceWords) =>
        _wordsFilters.Aggregate(
            sourceWords.AsResult(),
            (toFilter, filter) => toFilter.Then(filter.FilterWords)
        );
}