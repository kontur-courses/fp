using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ResultOf;
using TagsCloudContainer.Layouter;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Preprocessing
{
    public class LayouterApplicator
    {
        private readonly FontSettings fontSettings;
        private readonly Func<ITagCloudLayouter> layouterGenerator;

        public LayouterApplicator(Func<ITagCloudLayouter> layouterGenerator, FontSettings fontSettings)
        {
            this.layouterGenerator = layouterGenerator;
            this.fontSettings = fontSettings;
        }

        public Point WordsCenter { get; private set; }

        public Result<WordInfo[]> GetWordsAndRectangles(WordInfo[] wordsAndFrequencies)
        {
            if (wordsAndFrequencies == null)
                return Result.Fail<WordInfo[]>(nameof(wordsAndFrequencies) + " must be not null");
            var layouter = layouterGenerator();
            WordsCenter = layouter.Center;

            return Result.Of(() => GetRectangles(wordsAndFrequencies, layouter))
                .RefineError("Invalid font settings");
        }

        private WordInfo[] GetRectangles(WordInfo[] wordsAndFrequencies, ITagCloudLayouter layouter)
        {
            var wordInfos = new List<WordInfo>();
            foreach (var wordAndFrequency in wordsAndFrequencies)
            {
                var word = wordAndFrequency.Word;
                var frequency = wordAndFrequency.Frequency;
                var font = new Font(fontSettings.Font.FontFamily, frequency * fontSettings.FontSizeFactor);
                var size = TextRenderer.MeasureText(word, font);
                wordAndFrequency.FontSize = font.Size;
                wordAndFrequency.Rect = layouter.PutNextRectangle(size);
                wordInfos.Add(wordAndFrequency);
            }

            return wordInfos.ToArray();
        }
    }
}