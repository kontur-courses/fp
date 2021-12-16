using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.Settings
{
    public interface ISpeechPartColorMapSettings
    {
        IReadOnlyDictionary<SpeechPart, Color> ColorMap { get; set; }
    }
}