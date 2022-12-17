namespace TagCloudContainer.Formatters
{
    public class WordFormatter : IWordFormatter
    {
        public Result<IEnumerable<string>> Normalize(IEnumerable<string> textWords, Func<string, string> normalizeFunction)
        {
            return textWords.Select(normalizeFunction).ToList();
        }
    }
}
