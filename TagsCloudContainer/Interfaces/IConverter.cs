using ResultOfTask;

namespace TagsCloudContainer.Interfaces;

public interface IConverter
{
    Result<Dictionary<string, int>> GetWordsInFile(ICustomOptions options);
}