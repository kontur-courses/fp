using ResultOfTask;

namespace TagsCloudContainer.Interfaces;

public interface IWordsFilter
{
    public Result<List<string>> FilterWords(List<string> taggedWords,
        ICustomOptions options, HashSet<string>? boringWords);
}