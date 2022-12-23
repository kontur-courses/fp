namespace TagCloud.App.WordPreprocessorDriver.InputStream.TextSplitters;

public class NewLineTextSplitter : ITextSplitter
{
    public Result<List<string>> GetSplitWords(string text)
    {
        return Result.Of(() => text.Split(Environment.NewLine).ToList());
    }
}