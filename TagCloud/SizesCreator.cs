using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ExceptionHandler;

namespace TagCloud
{
    public static class SizesCreator
    {
        public static Result<SizeWithWord[]> CreateSizesArray(IEnumerable<string> words, float fontSize, string fontName)
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var fontResult = Result.Of(() => new Font(fontName, fontSize), $"Не был найден шрифт с именем: {fontName}");
            var frequencyDict = GetFrequencyDictionary(words);
            var maxSizeResult = fontResult.Then(font => frequencyDict.Select(pair => g.MeasureString(pair.Key, font)))
                    .Then(unsortedSizes => unsortedSizes.Max());
            var minFrequency = frequencyDict.Min(pair => pair.Value);
            var sizes = new List<SizeWithWord>(frequencyDict.Count);
            if (!maxSizeResult.IsSuccess)
                return Result.Fail<SizeWithWord[]>(maxSizeResult.Error);
            foreach (var pair in frequencyDict)
            {
                var weight = (double) pair.Value / minFrequency;
                sizes.Add(new SizeWithWord((maxSizeResult.GetValueOrThrow() * (float) weight).ToSize(), new Word(pair.Key, weight)));
            }

            return sizes.ToArray().AsResult();
        }

        private static Dictionary<string, int> GetFrequencyDictionary(IEnumerable<string> words)
        {
            var frequencyDict = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (frequencyDict.ContainsKey(word))
                {
                    frequencyDict[word] += 1;
                }
                else
                {
                    frequencyDict[word] = 1;
                }
            }

            return frequencyDict;
        }
    }
}