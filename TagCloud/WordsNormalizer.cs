namespace TagCloud;

public class WordsNormalizer : IWordsNormalizer
{
    public Result<List<string>> NormalizeWords(Result<List<string>> wordsResult, Result<HashSet<string>> boringWords)
    {
        return !boringWords.IsSuccess
            ? Result.Fail<List<string>>(boringWords.Error)
            : wordsResult
                .Then(word => word.Where(x => !boringWords.Value.Contains(x)).Select(x => x.ToLower()).ToList())
                .ReplaceErrorIfEmpty( "Can't normalize words");
    }
}