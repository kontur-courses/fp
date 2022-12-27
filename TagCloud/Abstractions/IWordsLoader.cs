using FluentResults;

namespace TagCloud.Abstractions;

public interface IWordsLoader
{
    Result<IEnumerable<string>> Load();
}