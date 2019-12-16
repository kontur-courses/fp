namespace TextConfiguration.WordProcessors
{
    public interface IWordProcessor
    {
        Result<string> ProcessWord(string word);
    }
}
