using Result;

namespace TagsCloudContainer.Interfaces;

public interface IWordsFilter
{
    public Result<List<string>> FilterWords(List<string> taggedWords,
        Result<ICustomOptions> options, HashSet<string>? boringWords);
}