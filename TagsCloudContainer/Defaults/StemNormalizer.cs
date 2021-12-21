using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults;

public class StemNormalizer : IWordNormalizer
{
    private readonly MyStem.MyStem myStem;

    public StemNormalizer(MyStem.MyStem myStem)
    {
        this.myStem = myStem;
    }

    public Result<string> Normalize(string word)
    {
        return myStem.AnalyzeWord(word)
            .Then(stat => stat.Stem)
            .RefineError($"Could not normalize word '{word}'");
    }
}
