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
        private readonly PathSettings pathSettings;

        public TagsHelper(IReaderFactory readerFactory, TextAnalyzer analyzer, PathSettings pathSettings)
        {
            this.readerFactory = readerFactory;
            this.analyzer = analyzer;
            this.pathSettings = pathSettings;
        }

        public Result<List<(string, int)>> GetWords()
        {
            return GetTextFromFile(pathSettings.PathToText)
                .Then(words => words.Select(x => x.ToLower()))
                .Then(ExcludeBoringWords)
                .Then(words => analyzer.GetWordByFrequency(words,
                x => x.OrderByDescending(y => y.Value)
                    .ThenByDescending(y => y.Key.Length)
                    .ThenBy(y => y.Key, StringComparer.Ordinal)));
        }

        private Result<List<string>> ExcludeBoringWords(IEnumerable<string> text)
        {
            return GetTextFromFile(pathSettings.PathToBoringWords)
                .Then(words => new HashSet<string>(words))
                .Then(words => text.Where(x => !words.Contains(x)).ToList());
        }

        private Result<List<string>> GetTextFromFile(string document)
        {
            var extension = document.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Last();
            return readerFactory.GetReader(extension)
                .Then(x => x.ReadWords(document));
        }

        public Result<Dictionary<int, Font>> CreateFonts(List<(string, int)> words, Font font)
        {
            return Result.Of(() => words.Select(x => x.Item2))
                .Then(frequency => frequency.GroupBy(x => x))
                .Then(groups => groups.ToDictionary(x => x.Key, 
                    x => new Font(font.FontFamily, (int) (font.Size * Math.Log(x.Key + 1)), font.Style)));
        }

        public Result<List<Result<Rectangle>>> GetRectangles(ICircularCloudLayouter cloud, 
            List<(string, int)> words, DisposableDictionary<int, Font> fonts)
        {
            return words
                .Select(x => cloud
                    .PutNextRectangle(new Size((int) fonts[x.Item2].Size * x.Item1.Length, fonts[x.Item2].Height)))
                .ToList();
        }
    }
}