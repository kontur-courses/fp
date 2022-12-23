using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;

public class BoringUnionsAndAppealsRu : IBoringWords
{
    private static readonly List<string> Unions = new()
    {
        "и", "или", "а", "с", "при", "но", "однако"
    };
    private static readonly List<string> Prepositions = new()
    {
        "в", "из", "к", "у", "по", "из-за", "по-над", "под", "около", "вокруг", "перед", "возле"
    };

    private static readonly List<string> Appeals = new()
    {
        "он", "она", "оно", "они", "им", "ей", "ему", "её", "его", "их"
    };
        
    public Result<None> IsBoring(IWord word)
    {
        return Result.Of(() => word.Value)
            .Then(wordValue =>
                Unions.Contains(wordValue)
                || Prepositions.Contains(wordValue)
                || Appeals.Contains(wordValue))
            .Then(result => result
                ? Result.Ok()
                : Result.Fail<None>("Word is not boring"));
    }
}