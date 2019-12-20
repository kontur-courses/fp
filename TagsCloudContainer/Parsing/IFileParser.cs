using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Parsing
{
    public interface IFileParser
    {
        Result<string[]> ParseFile(string filePath);
    }
}