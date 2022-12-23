using TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.Words;

namespace TagCloud.App.WordPreprocessorDriver.WordsPreprocessor.BoringWords;

public interface IBoringWords
{ 
    Result<None> IsBoring(IWord word);
}