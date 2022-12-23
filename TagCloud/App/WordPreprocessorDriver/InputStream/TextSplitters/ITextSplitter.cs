namespace TagCloud.App.WordPreprocessorDriver.InputStream.TextSplitters;

public interface ITextSplitter
{
    Result<List<string>> GetSplitWords(string text);
}