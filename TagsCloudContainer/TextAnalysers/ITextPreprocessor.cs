namespace TagsCloudContainer.TextAnalysers;

public interface ITextPreprocessor
{
    public Result<WordDetails[]> Preprocess(string text);
}