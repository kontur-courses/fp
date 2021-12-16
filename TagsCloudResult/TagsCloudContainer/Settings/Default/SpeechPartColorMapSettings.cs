using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Preprocessing;

namespace TagsCloudContainer.Settings.Default
{
    public class SpeechPartColorMapSettings : ISpeechPartColorMapSettings
    {
        public IReadOnlyDictionary<SpeechPart, Color> ColorMap { get; set; } = new Dictionary<SpeechPart, Color>();
    }
}