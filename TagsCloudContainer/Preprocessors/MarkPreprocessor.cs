using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Preprocessors;

public class MarkPreprocessor : IPreprocessor
{
    public string Preprocess(string word)
    {
        return new string(word.Where(char.IsLetter).ToArray());
    }
}