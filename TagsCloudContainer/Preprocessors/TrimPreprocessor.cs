namespace TagsCloudContainer.Preprocessors;

public class TrimPreprocessor : IPreprocessor
{
    public string Preprocess(string word)
    {
        return word.Trim();
    }
}
