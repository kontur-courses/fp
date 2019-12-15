using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;
using TagsCloudLibrary.Colorers;
using TagsCloudLibrary.Layouters;
using TagsCloudLibrary.Preprocessors;
using TagsCloudLibrary.Readers;
using TagsCloudLibrary.WordsExtractor;
using TagsCloudLibrary.Writers;

namespace TagsCloudLibrary
{
    public class TagsCloudGenerator
    {
        private readonly IReader reader;
        private readonly IWordsExtractor extractor;
        private readonly List<IPreprocessor> preprocessors;
        private readonly ILayouter layouter;
        private readonly IColorer colorer;
        private readonly FontFamily wordsFontFamily;
        private readonly IImageWriter imageWriter;

        public TagsCloudGenerator(
            IReader reader,
            IWordsExtractor extractor,
            IEnumerable<IPreprocessor> preprocessors,
            ILayouter layouter,
            IColorer colorer,
            FontFamily wordsFontFamily,
            IImageWriter imageWriter)
        {
            this.reader = reader;
            this.extractor = extractor;

            this.preprocessors = preprocessors.ToList();
            this.preprocessors = this.preprocessors.OrderBy(p => p.Priority).ToList();

            this.layouter = layouter;
            this.colorer = colorer;
            this.wordsFontFamily = wordsFontFamily;
            this.imageWriter = imageWriter;
        }

        private Result<Stream> OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
                return Result.Failure<Stream>($"File {fileName} not found");
            try
            {
                return Result.Ok<Stream>(File.OpenRead(fileName));
            }
            catch (Exception e)
            {
                return Result.Failure<Stream>(e.Message);
            }
        }

        private static Dictionary<string, int> CountWords(IEnumerable<string> words)
        {
            return words.Aggregate(new Dictionary<string, int>(), (stats, word) =>
            {
                if (stats.ContainsKey(word))
                    ++stats[word];
                else
                    stats[word] = 1;
                return stats;
            });
        }

        private IEnumerable<string> ApplyPreprocessors(IEnumerable<string> words)
        {
            return preprocessors.Aggregate(words, (wordsAccumulator, preprocessor) => preprocessor.Act(wordsAccumulator));
        }

        private Bitmap DrawCountedWords(int width, int height, List<Tuple<string, int>> wordsWithCounts)
        {
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            var totalWords = wordsWithCounts.Aggregate(0, (i, tuple) => i + tuple.Item2);

            foreach (var wordWithCount in wordsWithCounts)
            {
                var (word, count) = wordWithCount;
                var font = new Font(wordsFontFamily, (float)count * height / totalWords * 2);
                var color = colorer.ColorForWord(word, (double)count / totalWords);

                var textSize = graphics.MeasureString(word, font).ToSize();

                var (_, isFailure, rectangle) = layouter.PutNextRectangle(textSize);
                if (isFailure) continue;
                graphics.DrawString(
                    word,
                    font,
                    new SolidBrush(color),
                    rectangle.X + width / 2,
                    rectangle.Y + height / 2);
            }

            return image;
        }

        public Result GenerateFromFile(string inputFile, string outputFile, int imageWidth, int imageHeight)
        {

            return OpenFile(inputFile)
                .Map(fs => reader.Read(fs))
                .Bind(ds => extractor.ExtractWords(ds))
                .Map(ApplyPreprocessors)
                .Map(CountWords)
                .Map(statistics => statistics
                    .Select(pair => new Tuple<string, int>(pair.Key, pair.Value))
                    .OrderByDescending(tuple => tuple.Item2)
                    .ThenBy(tuple => tuple.Item1)
                    .ToList())
                .Map(wws => DrawCountedWords(imageWidth, imageHeight, wws))
                .Bind(image => imageWriter.WriteBitmapToFile(image, outputFile));
        }
    }
}
