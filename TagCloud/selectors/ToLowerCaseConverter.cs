namespace TagCloud.selectors
{
    public class ToLowerCaseConverter : IConverter<string>
    {
        public Result<string> Convert(string source) => source.ToLower();
    }
}