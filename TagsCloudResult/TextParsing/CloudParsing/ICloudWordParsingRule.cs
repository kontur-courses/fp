namespace TagsCloudResult.TextParsing.CloudParsing
{
    public interface ICloudWordParsingRule
    {
        bool Check(string word);
        string Apply(string word);
    }
}