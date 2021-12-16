using System.Collections.Generic;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.Settings
{
    public interface ISpeechPartFilterSettings
    {
        IEnumerable<SpeechPart> SpeechPartsToRemove { get; set; }
    }
}