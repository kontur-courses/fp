using TagsCloudContainer.TokensAndSettings;

namespace TagsCloudContainer.WordPreprocessors
{
    public interface IWordPreprocessor
    {
        Result<ProcessedWord[]> PreprocessWords(string[] text);
    }
}
