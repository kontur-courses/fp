namespace TextConfiguration.TextReaders
{
    public interface ITextReader
    {
        Result<string> ReadText(string filePath);
    }
}
