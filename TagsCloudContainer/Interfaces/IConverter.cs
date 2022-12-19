using Result;

namespace TagsCloudContainer.Interfaces;

public interface IConverter
{
    Result<Dictionary<string, int>> GetWordsInFile(Result<ICustomOptions> options);
}