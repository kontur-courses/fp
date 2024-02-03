using TagCloud.Domain.Settings;
using TagCloud.Domain.WordProcessing.Interfaces;

namespace TagCloud.Domain.WordProcessing.Extractors;

public class ExcludeExtractor : IWordExtractor
{
    private readonly WordSettings _wordSettings;
    
    public ExcludeExtractor(WordSettings wordSettings)
    {
        _wordSettings = wordSettings;
    }
    
    public bool IsSuitable(string word)
    {
        return _wordSettings.Excluded.All(excluded => !excluded.Equals(word, StringComparison.InvariantCulture));
    }
}