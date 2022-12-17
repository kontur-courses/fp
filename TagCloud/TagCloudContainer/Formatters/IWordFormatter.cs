namespace TagCloudContainer.Formatters
{
    public interface IWordFormatter
    {
        Result<IEnumerable<string>> Normalize(IEnumerable<string> textWords, Func<string, string> normalizeFunction);
    }
}
