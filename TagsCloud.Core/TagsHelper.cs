using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NHunspell;
using TagsCloud.Common;
using TagsCloud.FileReader;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class TagsHelper
    {
        private readonly IReaderFactory readerFactory;

        public TagsHelper(IReaderFactory readerFactory)
        {
            this.readerFactory = readerFactory;
        }

        public List<(string, int)> GetWords(string pathToFile, string pathToBoringWords,
            string pathToDictionary, string pathToAffix)
        {
            var mainText = GetTextFromFile(pathToFile).GetValueOrThrow();
            var boringWords = new HashSet<string>(GetTextFromFile(pathToBoringWords).GetValueOrThrow());
            var hunspell = Result.Of(() => new Hunspell(pathToAffix, pathToDictionary)).GetValueOrThrow();

            return TextAnalyzer.GetWordByFrequency(
                mainText,
                boringWords,
                hunspell,
                x => x.OrderByDescending(y => y.Value)
                    .ThenByDescending(y => y.Key.Length)
                    .ThenBy(y => y.Key, StringComparer.Ordinal));
        }

        private Result<string[]> GetTextFromFile(string document)
        {
            var extension = document.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Last();
            return readerFactory.GetReader(extension)
                .Then(x => x.ReadWords(document));
        }

        public List<Rectangle> GetRectangles(ICircularCloudLayouter cloud,
            List<(string, int)> words, Dictionary<int, Font> newFonts, Font font)
        {
            var rectangles = new List<Rectangle>();
            foreach (var (word, fontSize) in words)
            {
                if (!newFonts.TryGetValue(fontSize, out var newFont))
                {
                    newFont = new Font(font.FontFamily, (int) (font.Size * Math.Log(fontSize + 1)), font.Style);
                    newFonts[fontSize] = newFont;
                }

                var rect = cloud
                    .PutNextRectangle(new Size((int)newFont.Size * word.Length, newFont.Height))
                    .GetValueOrThrow();
                rectangles.Add(rect);
            }
            return rectangles;
        }
    }
}