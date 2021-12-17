using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults;

public class LowerNormalizer : IWordNormalizer
{
    public Result<string> Normalize(string word)
    {
        return word.ToLower();
    }
}
