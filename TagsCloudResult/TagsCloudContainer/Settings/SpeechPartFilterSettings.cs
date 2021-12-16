using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class SpeechPartFilterSettings : ISpeechPartFilterSettings
    {
        public IEnumerable<SpeechPart> SpeechPartsToRemove { get; set; } = Enumerable.Empty<SpeechPart>();
        //
        // public SpeechPartFilterSettings(RenderSettings settings)
        // {
        //     SpeechPartsToRemove = settings.IgnoredSpeechParts;
        // }
    }
}