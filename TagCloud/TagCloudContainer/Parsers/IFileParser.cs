namespace TagCloudContainer.Parsers
{
    public interface IFileParser
    {
        Result<IEnumerable<string>> Parse(string text, string separator);
    }
}
