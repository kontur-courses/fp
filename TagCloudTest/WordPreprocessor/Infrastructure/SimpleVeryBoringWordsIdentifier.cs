using TagCloud;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloudTest.WordPreprocessor.Infrastructure;

public class SimpleVeryBoringWordsIdentifier : IBoringWords
{
    public Result<None> IsBoring(IWord word)
    {
        return word.Value == "very-boring"
            ? Result.Ok()
            : Result.Fail<None>("Word is not boring");
    }
}