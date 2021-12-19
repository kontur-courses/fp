using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Preprocessors;

public class LowercasePreprocessor : IPreprocessor
{
    public string Preprocess(string word)
    {
        return word.ToLower();
    }
}