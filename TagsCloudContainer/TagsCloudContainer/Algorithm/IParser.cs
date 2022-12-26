using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public interface IParser
    {
        Result<Dictionary<string, int>> CountWordsInFile(string pathToFile);
        Result<HashSet<string>> FindWordsInFile(string pathToFile);
    }
}
