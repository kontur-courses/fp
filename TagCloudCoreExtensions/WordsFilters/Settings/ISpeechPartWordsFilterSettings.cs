using MyStemWrapper.Domain;

namespace TagCloudCoreExtensions.WordsFilters.Settings;

public interface ISpeechPartWordsFilterSettings
{
    ISet<SpeechPart> ExcludedSpeechParts { get; set; }
    bool ExcludeUndefined { get; set; }
}