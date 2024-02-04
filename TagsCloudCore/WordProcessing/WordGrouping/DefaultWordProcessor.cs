using TagsCloudCore.BuildingOptions;
using TagsCloudCore.WordProcessing.WordFiltering;
using TagsCloudCore.WordProcessing.WordInput;

namespace TagsCloudCore.WordProcessing.WordGrouping;

public class DefaultWordProcessor : IProcessedWordProvider
{
    private readonly IEnumerable<IWordFilter> _filters;
    private readonly CommonOptions _options;
    private readonly IEnumerable<IWordProvider> _wordProviders;

    public DefaultWordProcessor(ICommonOptionsProvider commonOptionsProvider, IEnumerable<IWordFilter> filters,
        IEnumerable<IWordProvider> wordProviders)
    {
        _options = commonOptionsProvider.CommonOptions;
        _wordProviders = wordProviders;
        _filters = filters;
    }

    public Result<IReadOnlyDictionary<string, int>> ProcessWords()
    {
        var provider = _wordProviders.SingleOrDefault(p => p.Match(_options.WordProviderInfo.Type));
        var getWordsResult = provider!.GetWords(_options.WordProviderInfo.ResourceLocation);
        if (!getWordsResult.IsSuccess)
            return Result.Fail<IReadOnlyDictionary<string, int>>(getWordsResult.Error);
        
        var words = getWordsResult.Value.Select(w => w.Trim()).Select(w => w.ToLower()).ToArray();
        
        foreach (var filter in _filters)
        {
            var filterResult = filter.FilterWords(words, _options.WordFilterInfo);
            if (!filterResult.IsSuccess)
                return Result.Fail<IReadOnlyDictionary<string, int>>(filterResult.Error);
            words = filterResult.Value;
        }
        return GroupWords(words);
    }

    private static Dictionary<string, int> GroupWords(IEnumerable<string> filtered)
    {
        var frequency = new Dictionary<string, int>();
        foreach (var word in filtered)
        {
            frequency.TryAdd(word, 0);
            frequency[word]++;
        }

        return frequency;
    }
}