using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.Analyzer;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.DefinerFontSize
{
    public class DefinerFontSize : IDefinerFontSize
    {
        private readonly FontSettings fontSettings;

        public DefinerFontSize(FontSettings fontSettings)
        {
            this.fontSettings = fontSettings;
        }

        public Result<IEnumerable<WordWithFont>> DefineFontSize(IEnumerable<IWeightedWord> words)
        {
            var weightedWords = words.ToArray();

            if (weightedWords.Length == 0) return Result.Fail<IEnumerable<WordWithFont>>("пустой список слов");

            var countWord = weightedWords.Max(p => p.Weight);
            var difference = fontSettings.MaxEmSize - fontSettings.MinEmSize;

            return Result.Ok(weightedWords.Select(word =>
            {
                var percent = word.Weight / (float)countWord;
                var emSize = fontSettings.MinEmSize + difference * percent;
                return new WordWithFont
                {
                    Font = new Font(fontSettings.FontFamily, emSize, fontSettings.Style),
                    Word = word.Word
                };
            }));
        }
    }
}