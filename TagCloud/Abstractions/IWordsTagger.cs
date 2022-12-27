using FluentResults;

namespace TagCloud.Abstractions;

public interface IWordsTagger
{
    Result<IEnumerable<ITag>> ToTags(IEnumerable<string> words);
}