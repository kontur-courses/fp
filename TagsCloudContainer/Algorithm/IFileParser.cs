using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public interface IFileParser
    {
        Result<List<string>> ReadWordsInFile(string filePath);
    }
}