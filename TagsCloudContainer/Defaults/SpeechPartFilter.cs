using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.SettingsProviders;

namespace TagsCloudContainer.Defaults;

public class SpeechPartFilter : IWordFilter
{
    private readonly SpeechPartFilterSettings settings;
    private readonly MyStem.MyStem myStem;

    public SpeechPartFilter(SpeechPartFilterSettings settings, MyStem.MyStem myStem)
    {
        this.settings = settings;
        this.myStem = myStem;
    }

    public Result<bool> IsValid(string word)
    {
        return myStem.AnalyzeWord(word)
            .Then(stat => stat.SpeechPart)
            .Then(part => !settings.ToFilterOut.Contains(part));
    }
}
