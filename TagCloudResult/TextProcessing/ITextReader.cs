namespace TagCloudResult.TextProcessing
{
    public interface ITextReader
    {
        Result<IEnumerable<string>> GetWordsFrom(string resource);
    }
}
