namespace TagCloudContainer.Parsers
{
    public class FileLinesParser : IFileParser
    {
        public Result<IEnumerable<string>> Parse(string text, string separator)
        {
            var parsedText = text.Split(separator);
            return parsedText.Length <= 1 
                ? new Result<IEnumerable<string>>("Could not parse file for parameters") 
                : parsedText;
        }
    }
}
