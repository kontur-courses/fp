using TagsCloudContainer.TokensAndSettings;

namespace TagsCloudContainer.WordPreprocessors
{
    public interface IWordPreprocessor
    {
        Result<ProcessedWord[]> WordPreprocessing(string[] text);
    }
}
