using FluentResults;

namespace TagCloud.Abstractions;

public interface IWordsProcessor
{
    Result<IEnumerable<string>> Process(IEnumerable<string> words);
}