using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.Settings.Default
{
    public class SpeechPartFilterSettings : ISpeechPartFilterSettings
    {
        public IEnumerable<SpeechPart> SpeechPartsToRemove { get; set; } = Enumerable.Empty<SpeechPart>();
    }
}