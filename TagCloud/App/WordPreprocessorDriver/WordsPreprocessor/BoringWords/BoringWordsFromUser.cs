using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;

public class BoringWordsFromUser : IBoringWords
{
    private readonly List<string> usersWords;
    
    public BoringWordsFromUser()
    {
        usersWords = new List<string>();
    }

    public Result<None> IsBoring(IWord word)
    {
        return Result.Of(() =>
            usersWords.Any(boringWord => boringWord == word.Value))
            .Then(result => result
                ? Result.Ok()
                : Result.Fail<None>("Word is not boring"));
    }

    public Result<None> AddBoringWord(string word)
    {
        return Result.OfAction(() => usersWords.Add(word));
    }
}