using MyStemWrapper;
using TagCloud.Excluders;
using TagCloud.Extensions;

namespace TagCloud.TextHandlers;

public class FileTextHandler : ITextHandler
{
    private readonly Stream stream;
    private readonly IWordFilter filter;

    public FileTextHandler(Stream stream, IWordFilter filter)
    {
        this.stream = stream;
        this.filter = filter;
    }

    public Result<Dictionary<string, int>> Handle()
    {
        using var sr = new StreamReader(stream);

        return EnumerableExtension
            .RepeatUntilNull(sr.ReadLine)
            .CountValues()
            .AsResult()
            .Then(filter.ExcludeWords!);
    }
}