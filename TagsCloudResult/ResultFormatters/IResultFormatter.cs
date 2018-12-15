namespace TagsCloudResult.ResultFormatters
{
    public interface IResultFormatter
    {
        Result<None> GenerateResult(string outputFileName);
    }
}
