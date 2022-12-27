using FluentResults;
using TagCloud.Abstractions;

namespace TagCloud;

public class TxtLinesWordsLoader : IWordsLoader
{
    private readonly string filepath;

    public TxtLinesWordsLoader(string filepath)
    {
        this.filepath = filepath;
    }

    public Result<IEnumerable<string>> Load()
    {
        if (!File.Exists(filepath))
            return Result.Fail(new Error($"Could not find file '{Path.GetFullPath(filepath)}'."));

        var extension = Path.GetExtension(filepath);
        if (extension != ".txt")
            return Result.Fail(new Error($"Only files extension '.txt' are supported, but '{extension}'."));
        
        return Result.Try(() => File.ReadAllLines(filepath).Select(s => s));
    }
}