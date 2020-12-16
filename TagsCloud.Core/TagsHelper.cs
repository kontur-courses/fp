using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Common;
using TagsCloud.FileReader;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class TagsHelper
    {
        private readonly IReaderFactory readerFactory;
        private readonly TextAnalyzer analyzer;

        public TagsHelper(IReaderFactory readerFactory, TextAnalyzer analyzer)
        {
            this.readerFactory = readerFactory;
            this.analyzer = analyzer;
        }

        public Result<List<(string, int)>> GetWords(string pathToFile, string pathToBoringWords)
        {
            return GetTextFromFile(pathToFile)
                .Then(words => words.Select(x => x.ToLower()))
                .Then(words => ExcludeBoringWords(words, pathToBoringWords))
                .Then(words => analyzer.GetWordByFrequency(words,
                x => x.OrderByDescending(y => y.Value)
                    .ThenByDescending(y => y.Key.Length)
                    .ThenBy(y => y.Key, StringComparer.Ordinal)));
        }

        private Result<List<string>> ExcludeBoringWords(IEnumerable<string> text, string pathToBoringWords)
        {
            return GetTextFromFile(pathToBoringWords)
                .Then(words => new HashSet<string>(words))
                .Then(words => text.Where(x => !words.Contains(x)).ToList());
        }

        private Result<List<string>> GetTextFromFile(string document)
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