using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CTV.Common;

namespace CTV.Common
{
    public class FrequencyBasedWordSizer : IWordSizer
    {
        public List<SizedWord> Convert(string[] words, Font font, Func<string, Font, Size> getWordSize)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));
            if (words == null)
                throw new ArgumentNullException(nameof(words));

            var wordToFrequency = CalculateWordsFrequency(words);
            var wordToCustomFont = CalculateFontSizes(wordToFrequency, font);

            return wordToCustomFont.Select(kv =>
                    new SizedWord(
                        kv.Key,
                        kv.Value,
                        getWordSize(kv.Key, kv.Value)))
                .ToList();
        }

        private static Dictionary<string, Font> CalculateFontSizes(
            Dictionary<string, float> wordToFrequency,
            Font font)
        {
            return wordToFrequency.ToDictionary(
                kv => kv.Key,
                kv => new Font(font.FontFamily, kv.Value * font.SizeInPoints, font.Style));
        }

        private static Dictionary<string, float> CalculateWordsFrequency(string[] words)
        {
            return words
                .GroupBy(x => x)
                .ToDictionary(x => x.Key,
                    y => (float) y.Count() / words.Length);
        }
    }
}