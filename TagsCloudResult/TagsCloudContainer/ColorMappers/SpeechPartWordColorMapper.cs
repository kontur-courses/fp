using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Results;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.ColorMappers
{
    public class SpeechPartWordColorMapper : IWordColorMapper
    {
        public WordColorMapperType Type => WordColorMapperType.SpeechPart;

        private readonly IWordSpeechPartParser wordSpeechPartParser;
        private readonly ISpeechPartColorMapSettings colorMapSettings;
        private readonly IDefaultColorSettings defaultColor;

        public SpeechPartWordColorMapper(
            IWordSpeechPartParser wordSpeechPartParser,
            ISpeechPartColorMapSettings colorMapSettings,
            IDefaultColorSettings defaultColor)
        {
            this.wordSpeechPartParser = wordSpeechPartParser;
            this.colorMapSettings = colorMapSettings;
            this.defaultColor = defaultColor;
        }

        public Result<Dictionary<WordLayout, Color>> GetColorMap(CloudLayout layout)
        {
            var words = layout.WordLayouts.Select(wordLayout => wordLayout.Word);

            return wordSpeechPartParser.ParseWords(words)
                .Then(speechPartWords => speechPartWords
                    .Select(speechPartWord => speechPartWord.SpeechPart))
                .Then(speechParts =>
                    FillMap(layout.WordLayouts.Zip(speechParts)));
        }

        private Dictionary<WordLayout, Color> FillMap(
            IEnumerable<(WordLayout WordLayout, SpeechPart SpeechPart)> layoutSpeechPartPairs)
        {
            return layoutSpeechPartPairs.ToDictionary(
                pair => pair.WordLayout,
                pair => SelectColor(pair.SpeechPart));
        }

        private Color SelectColor(SpeechPart speechPart)
        {
            return colorMapSettings.ColorMap.TryGetValue(speechPart, out var color)
                ? color
                : defaultColor.Color;
        }
    }
}