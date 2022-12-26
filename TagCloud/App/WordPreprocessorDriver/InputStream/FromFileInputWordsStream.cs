using TagCloud.App.WordPreprocessorDriver.InputStream.TextSplitters;

namespace TagCloud.App.WordPreprocessorDriver.InputStream;

public class FromFileInputWordsStream
{
    private readonly ITextSplitter textSplitter;

    public FromFileInputWordsStream(ITextSplitter textSplitter)
    {
        this.textSplitter = textSplitter;
    }

    public Result<List<string>> GetAllWordsFromStream(
        FromFileStreamContext streamContext)
    {
        var fn = streamContext.Filename;
        return Result.FailIf(fn, !File.Exists(fn), $"File was not found at {fn}")
            .Then(_ => FillWordsFromFile(streamContext.Filename, streamContext.FileEncoder, textSplitter))
            .RefineError("Can not get text from stream");
    }

    private static Result<List<string>> FillWordsFromFile(string filename, IFileEncoder fileEncoder,
        ITextSplitter splitter)
    {
        return fileEncoder.GetText(filename).RefineError("Can not get data from file")
            .Then(splitter.GetSplitWords)
            .Then(words => words.Where(s => s.Length > 0).ToList());
    }
}