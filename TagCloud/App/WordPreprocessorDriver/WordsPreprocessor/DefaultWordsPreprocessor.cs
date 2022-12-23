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

    public Result<ISet<IWord>> GetProcessedWords(List<string> words, IReadOnlyCollection<IBoringWords> boringWords)
    {
        return CreateWordsSet(words)
            .Then(uniqueWords => Result.Of(() =>
                uniqueWords.Where(word =>
                        boringWords.All(checker => !checker.IsBoring(word)))
                    .ToHashSet()))
            .Then(uniqueWords => CalculateTfIndexes(uniqueWords, words.Count));
    }

    private static Result<double> GetTfIndex(int wordCount, int totalWordsCount)
    {
        return totalWordsCount <= 0
            ? Result.Fail<double>("Total words count can not be 0")
            : Result.Ok(1d * wordCount / totalWordsCount);
    }
        
    private static Result<ISet<IWord>> CalculateTfIndexes(ISet<IWord> words, int totalWordsCount)
    {
        foreach (var word in words)
        {
            var result = GetTfIndex(word.Count, totalWordsCount)
                .Then(value => word.Tf = value);
            if (!result.IsSuccess)
                return Result.Fail<ISet<IWord>>("Can not calculate tf indexes. " + result.Error);
        }
        return Result.Ok(words);
    }
        
    private Result<HashSet<IWord>> CreateWordsSet(IEnumerable<string> words)
    {
        var set = new HashSet<IWord>();
        return Result.OfAction(() =>
            {
                foreach (var word in words.Select(wordValue => new Word(wordValue.ToLower(cultureInfo))))
                {
                    if (set.TryGetValue(word, out var containedWord))
                        containedWord.Count++;
                    else
                        set.Add(word);
                }
            })
            .Then(_ => set);
    }
}