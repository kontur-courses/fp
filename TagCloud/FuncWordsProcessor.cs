using FluentResults;
using TagCloud.Abstractions;

namespace TagCloud;

public class FuncWordsProcessor : IWordsProcessor
{
    private readonly Func<IEnumerable<string>, Result<IEnumerable<string>>> process;

    public FuncWordsProcessor(Func<IEnumerable<string>, IEnumerable<string>> process)
    {
        this.process = words => Result.Try(() => process(words));
    }

    public Result<IEnumerable<string>> Process(IEnumerable<string> words)
    {
        return process(words);
    }
}