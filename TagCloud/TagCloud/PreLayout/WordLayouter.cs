using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagCloud.Drawing;
using TagCloud.Layout;

namespace TagCloud.PreLayout
{
    internal class WordLayouter : IWordLayouter
    {
        private readonly ICloudLayouter _layouter;

        public WordLayouter(ICloudLayouter layouter)
        {
            _layouter = layouter;
        }

        public List<Result<Word>> Layout(IDrawerOptions options, Dictionary<string, int> wordsWithFrequency)
        {
            var words = WordScaler
                .GetWordsWithScaledFontSize(wordsWithFrequency, options.BaseFontSize,
                    options.FontFamily);

            using var g = Graphics.FromImage(new Bitmap(1, 1));
            var wordsResult = words
                .Select(word =>
                {
                    var wordSize = g.MeasureString(word.Text, word.Font).ToSize();
                    return _layouter.PutNextRectangle(wordSize)
                        .Then(s => new Word(word, s));
                }).ToList();

            _layouter.Reset();
            return wordsResult;
        }
    }
}