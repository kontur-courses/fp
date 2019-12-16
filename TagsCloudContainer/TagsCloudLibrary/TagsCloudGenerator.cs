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
        private readonly IImageWriter imageWriter;
        private readonly TagsCloudGeneratorConfig tagsCloudGeneratorConfig;

        public TagsCloudGenerator(
            IReader reader,
            IWordsExtractor extractor,
            IEnumerable<IPreprocessor> preprocessors,
            ILayouter layouter,
            IColorer colorer,
            IImageWriter imageWriter,
            TagsCloudGeneratorConfig tagsCloudGeneratorConfig)
        {
            this.reader = reader;
            this.extractor = extractor;

            this.preprocessors = preprocessors.ToList();
            this.preprocessors = this.preprocessors.OrderBy(p => p.Priority).ToList();

            this.layouter = layouter;
            this.colorer = colorer;
            this.imageWriter = imageWriter;
            this.tagsCloudGeneratorConfig = tagsCloudGeneratorConfig;
        }
        public Result GenerateFromFile(string inputFile, string outputFile, int imageWidth, int imageHeight)
        {

            return Result.Ok()
                .Ensure(() => imageWidth > 0 && imageHeight > 0, "Image size is incorrect")
                .Ensure(() => File.Exists(inputFile), $"File {inputFile} not found")
                .Bind(() => reader.Read(inputFile))
                .Bind(extractor.ExtractWords)
                .Map(ApplyPreprocessors)
                .Map(CountWords)
                .Map(StatisticsDictionaryToList)
                .Bind(wws => DrawCountedWords(imageWidth, imageHeight, wws))
                .Bind(image => imageWriter.WriteBitmapToFile(image, outputFile));
        }

        private List<Tuple<string, int>> StatisticsDictionaryToList(Dictionary<string, int> statisticsDictionary) =>
            statisticsDictionary
                .Select(pair => new Tuple<string, int>(pair.Key, pair.Value))
                .OrderByDescending(tuple => tuple.Item2)
                .ThenBy(tuple => tuple.Item1)
                .ToList();

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

        private Result<Bitmap> CreateImage(int width, int height)
        {
            try
            {
                return Result.Ok(new Bitmap(width, height));
            }
            catch (Exception e)
            {
                return Result.Failure<Bitmap>("Failed to create image. " + e.Message);
            }
        }

        private Result<FontFamily> GetFontFamilyByName(string name)
        {
            try
            {
                return Result.Ok(new FontFamily(tagsCloudGeneratorConfig.FontFamilyName));
            }
            catch (Exception e)
            {
                return Result.Failure<FontFamily>(e.Message);
            }
        }

        private Result<Bitmap> DrawCountedWords(int width, int height, List<Tuple<string, int>> wordsWithCounts)
        {
            FontFamily fontFamily = null;
            var totalWords = wordsWithCounts.Aggregate(0, (i, tuple) => i + tuple.Item2);

            return
                Result.Ok()
                .Bind(() => GetFontFamilyByName(tagsCloudGeneratorConfig.FontFamilyName))
                .Tap(ff => fontFamily = ff)
                .Bind(ff => CreateImage(width, height))
                .Tap(bitmap =>
                {
                    var graphics = Graphics.FromImage(bitmap);
                    wordsWithCounts.ForEach(tuple =>
                    {
                        var (word, count) = tuple;
                        var font = new Font(fontFamily, (float)count * height / totalWords * 2);

                        layouter
                            .PutNextRectangle(graphics.MeasureString(word, font).ToSize())
                            .Tap(rectangle =>
                                graphics.DrawString(
                                word,
                                font,
                                new SolidBrush(colorer.ColorForWord(word, (double)count / totalWords)),
                                rectangle.X + width / 2,
                                rectangle.Y + height / 2));

                    });
                });
        }

        private Result<Stream> OpenFile(string fileName)
        {
            try
            {
                return Result.Ok<Stream>(File.OpenRead(fileName));
            }
            catch (Exception e)
            {
                return Result.Failure<Stream>(e.Message);
            }
        }
    }
}
