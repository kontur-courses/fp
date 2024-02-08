namespace TagsCloudContainer.TextAnalysers;

public interface IMyStemParser
{
    public Result<WordDetails> Parse(string wordInfo);
}