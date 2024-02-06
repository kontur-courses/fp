using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.FileReader.Interfaces;

public interface ITextReader
{
    public Result<string> GetTextFromFile(string filePath);
}