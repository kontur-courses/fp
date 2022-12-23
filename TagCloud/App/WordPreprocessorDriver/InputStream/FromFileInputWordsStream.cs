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
        return CheckFile(streamContext.Filename, streamContext.FileEncoder)
            .Then(_ => FillWordsFromFile(streamContext.Filename, streamContext.FileEncoder, textSplitter))
            .RefineError("Can not get text from stream");
    }

    private static Result<None> CheckFile(string filename, IFileEncoder selectedFileEncoder)
    {
        return Result.FailIf(filename, !File.Exists(filename), $"File was not found at {filename}")
            .Then(selectedFileEncoder.IsSuitableFileType);
    }

    private static Result<List<string>> FillWordsFromFile(string filename, IFileEncoder fileEncoder,
        ITextSplitter splitter)
    {
        return fileEncoder.GetText(filename).RefineError("Can not get data from file")
            .Then(text => splitter.GetSplitWords(text).RefineError("Can not split text"))
            .Then(words => Result.Of(() => words.Where(s => s.Length > 0).ToList())
                .RefineError("Can not select suitable words"));
    }
}