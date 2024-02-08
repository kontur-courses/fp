public class FontSizeCalculator
{
    private const int BaseFontSize = 30;
    private const int FontSizeMultiplier = 2;
    private const int DefaultFontSize = 10;

    public int CalculateWordFontSize(string word, Dictionary<string, int> wordFrequencies)
    {
        if (wordFrequencies.TryGetValue(word, out var frequency))
        {
            return Math.Max(BaseFontSize, BaseFontSize + frequency * FontSizeMultiplier);
        }
        return DefaultFontSize;
    }

    public Dictionary<string, int> CalculateWordFrequencies(IEnumerable<string> words)
    {
        return words.GroupBy(word => word)
            .ToDictionary(group => group.Key, group => group.Count());
    }
}