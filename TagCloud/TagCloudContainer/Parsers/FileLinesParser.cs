namespace TagCloudContainer.Parsers
{
    public class FileLinesParser : IFileParser
    {
        public Result<IEnumerable<string>> Parse(string text)
        {
            return Result.Of(()=> text.Split(Environment.NewLine)).Then(x=> x as IEnumerable<string>).RefineError("Error parse file");
        }
    }
}
