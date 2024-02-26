using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.FileReader;

public interface ITextReader
{
    public Result<string> GetTextFromFile(string filePath);
}
