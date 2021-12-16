using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class SpeechPartColorMapSettings : ISpeechPartColorMapSettings
    {
        public IReadOnlyDictionary<SpeechPart, Color> ColorMap { get; set; } = new Dictionary<SpeechPart, Color>();
        //
        // public SpeechPartColorMapSettings(RenderSettings settings)
        // {
        //     ColorMap = settings.SpeechPartColorMap;
        // }
    }
}