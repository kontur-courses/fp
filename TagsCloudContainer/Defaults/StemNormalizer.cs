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
        var result = myStem.AnalyzeWord(word);
        return result.IsSuccess
            ? result.GetValueOrThrow().Stem
            : Result.Fail<string>($"Could not normilize word '{word}'");
    }
}
