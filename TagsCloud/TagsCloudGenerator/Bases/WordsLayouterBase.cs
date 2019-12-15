using FailuresProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.Bases
{
    public abstract class WordsLayouterBase : IWordsLayouter
    {
        private readonly IRectanglesLayouter rectanglesLayouter;
        private readonly ISettings settings;
        private readonly Func<IEnumerable<(string word, int freq)>, IEnumerable<(string word, int freq)>> getEnumerableOrder;
        private readonly Func<(int freq, int maxFreq), int> getMaxSymbolSize;

        public WordsLayouterBase(
            IRectanglesLayouter rectanglesLayouter,
            ISettings settings,
            Func<IEnumerable<(string word, int freq)>, IEnumerable<(string word, int freq)>> getEnumerableOrder,
            Func<(int freq, int maxFreq), int> getMaxSymbolSize)
        {
            this.rectanglesLayouter = rectanglesLayouter;
            this.settings = settings;
            this.getEnumerableOrder = getEnumerableOrder;
            this.getMaxSymbolSize = getMaxSymbolSize;
        }

        public Result<(string word, float maxFontSymbolWidth, string fontName, RectangleF wordRectangle)[]> ArrangeWords(
            string[] words)
        {
            if (words == null)
                throw new ArgumentNullException();
            return
                rectanglesLayouter.Reset()
                .Then(none => CheckFont(settings.Font))
                .Then(none => ArrangeWords(words, settings.Font))
                .RefineError($"{GetType().Name} failure");
        }

        private Result<(string word, float maxFontSymbolWidth, string fontName, RectangleF wordRectangle)[]> ArrangeWords(
            string[] words, string fontName)
        {
            var result = new List<(
                string word,
                float maxFontSymbolWidth,
                string fontName,
                RectangleF wordRectangle)>();
            if (words.Length == 0)
                return Result.Ok(result.ToArray());
            var wordsFreq = CalculateWordsFrequency(words);
            var maxFreqCount = wordsFreq.Max(p => p.freq);
            var rectangleResult = Result.Ok<RectangleF>(default);
            foreach (var (word, freq) in getEnumerableOrder(wordsFreq))
            {
                var maxSymbolSize = getMaxSymbolSize((freq, maxFreqCount));
                using (var wordFont = new Font(fontName, maxSymbolSize))
                {
                    rectangleResult = rectanglesLayouter.PutNextRectangle(
                        new SizeF((word.Length + 1) * wordFont.SizeInPoints,
                        wordFont.Height));
                    if (!rectangleResult.IsSuccess)
                        break;
                    result.Add((word, maxSymbolSize, fontName, rectangleResult.Value));
                }
            }
            return
                rectangleResult
                .Then(rect => Result.Ok(result.ToArray()));
        }

        private Result<None> CheckFont(string fontName)
        {
            using (var font = new Font(fontName, 10))
                return 
                    font.Name.ToLower() == fontName.ToLower() ?
                    Result.Ok() :
                    Result.Fail<None>($"Font with name \'{fontName}\' not found in system");
        }

        private static IEnumerable<(string word, int freq)> CalculateWordsFrequency(string[] words) =>
            words
            .GroupBy(w => w)
            .Select(g => (g.Key, g.Count()));
    }
}