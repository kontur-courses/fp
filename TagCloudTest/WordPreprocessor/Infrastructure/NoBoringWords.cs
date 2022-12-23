using TagCloud;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;
using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloudTest.WordPreprocessor.Infrastructure;

public class NoBoringWords : IBoringWords
{
    public Result<None> IsBoring(IWord word)
    {
        return Result.Fail<None>("Word is not boring");
    }
}