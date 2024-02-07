namespace TagCloudGenerator.TextReaders
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> ReadTextFromFile(string filePath);
        string GetFileExtension();
    }
}