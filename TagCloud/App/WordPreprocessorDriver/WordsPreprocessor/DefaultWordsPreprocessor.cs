using System.Globalization;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.WordPreprocessorDriver.WordsPreprocessor;

public class DefaultWordsPreprocessor : IWordsPreprocessor
{
    private readonly CultureInfo cultureInfo;

    public DefaultWordsPreprocessor(CultureInfo cultureInfo)
    {
        this.cultureInfo = cultureInfo;
    }

    public ISet<IWord> GetProcessedWords(List<string> words, IReadOnlyCollection<IBoringWords> boringWords)
    {
        var uniqueWords = CreateWordsSet(words)
            .Where(word => boringWords.All(checker => !checker.IsBoring(word)))
            .ToHashSet();
        return CalculateTfIndexes(uniqueWords, words.Count);
    }

    private static double GetTfIndex(int wordCount, int totalWordsCount)
    {
        return 1d * wordCount / totalWordsCount;
    }

    private static ISet<IWord> CalculateTfIndexes(ISet<IWord> words, int totalWordsCount)
    {
        if (totalWordsCount == 0) return words;
        foreach (var word in words)
            word.Tf = GetTfIndex(word.Count, totalWordsCount);
        return words;
    }

    private ISet<IWord> CreateWordsSet(IEnumerable<string> words)
    {
        var set = new HashSet<IWord>();
        foreach (var word in words.Select(wordValue => new Word(wordValue.ToLower(cultureInfo))))
            if (!set.Add(word))
            {
                set.TryGetValue(word, out var savedWord);
                savedWord!.Count++; // never null in this context
            }
        return set;
    }
}