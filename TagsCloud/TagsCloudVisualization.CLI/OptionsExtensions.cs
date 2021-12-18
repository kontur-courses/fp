using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ResultMonad;
using TagsCloud.Utils;
using TagsCloudDrawer.ColorGenerators;
using TagsCloudDrawer.ImageSaveService;
using TagsCloudDrawer.ImageSettings;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.VectorsGenerator;
using TagsCloudVisualization.Drawable.Tags.Settings;
using TagsCloudVisualization.Drawable.Tags.Settings.TagColorGenerator;
using TagsCloudVisualization.WordsPreprocessor;
using TagsCloudVisualization.WordsProvider;

namespace TagsCloudVisualization.CLI
{
    public static class OptionsExtensions
    {
        private static string DictionariesDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Dictionaries");

        internal static Result<TagsCloudVisualisationSettings> ToDrawerSettings(this Options options)
        {
            return Result.Ok()
                .ValidateNonNull(options, nameof(options))
                .Then(() =>
                {
                    return
                        from wordsProvider in GetWordsFromFileProviderForFile(options.WordsFile)
                        from boringWords in GetExcludedWordsFromFile(options.ExcludingWordsFile)
                        from imageSettingsProvider in GetImageSettingsProvider(options)
                        from tagDrawableSettingsProvider in GetTagDrawableSettingsProvider(options)
                        from layouter in GetLayouter(options)
                        from imageSaveService in GetSaveServiceFromName(options)
                        from wordsPreprocessors in GetWordPreprocessors(options)
                        select new TagsCloudVisualisationSettings
                        {
                            WordsProvider = wordsProvider,
                            BoringWords = boringWords,
                            ImageSettingsProvider = imageSettingsProvider,
                            TagDrawableSettingsProvider = tagDrawableSettingsProvider,
                            Layouter = layouter,
                            ImageSaveService = imageSaveService,
                            WordsPreprocessors = wordsPreprocessors
                        };
                });
        }

        private static Result<IEnumerable<IWordsPreprocessor>> GetWordPreprocessors(Options options)
        {
            return options.Languages
                .DefaultIfEmpty("en")
                .Distinct()
                .Select(CreateInfinitiveFormProcessorFromDictionary)
                .Traverse();
        }


        private static Result<ImageSettingsProvider> GetImageSettingsProvider(Options options)
        {
            return
                from backgroundColor in ParseBackgroundColor(options)
                from imageSize in ParseSize(options)
                from value in ImageSettingsProvider.Create(backgroundColor, imageSize)
                select value;
        }

        private static Result<Color> ParseBackgroundColor(Options options) => Color.FromName(options.BackgroundColor);

        private static Result<Size> ParseSize(Options options) => new Size(options.Width, options.Height);

        private static Result<TagDrawableSettingsProvider> GetTagDrawableSettingsProvider(Options options)
        {
            return
                from colorGenerator in GetTagsColorGeneratorFromName(options.TagsColor)
                from fontSettings in FontSettings.Create(options.FontFamily, options.MaxFontSize)
                from value in TagDrawableSettingsProvider.Create(fontSettings, colorGenerator)
                select value;
        }

        private static Result<IWordsPreprocessor> CreateInfinitiveFormProcessorFromDictionary(string language)
        {
            var dictionaryPath = Path.Combine(DictionariesDirectory, language, "index.dic");
            var affixPath = Path.Combine(DictionariesDirectory, language, "index.aff");
            if (!File.Exists(dictionaryPath))
                return Result.Fail<IWordsPreprocessor>($"Cannot find file {dictionaryPath}");
            if (!File.Exists(affixPath))
                return Result.Fail<IWordsPreprocessor>($"Cannot find file {affixPath}");
            return Result.Of<IWordsPreprocessor>(() =>
            {
                using var dictionaryStream = File.OpenRead(dictionaryPath);
                using var affixStream = File.OpenRead(affixPath);
                return new ToInfinitiveFormProcessor(dictionaryStream, affixStream);
            }, $"Error when load dictionary for {language}");
        }

        private static Result<IEnumerable<string>> GetExcludedWordsFromFile(string filename) =>
            !File.Exists(filename)
                ? Enumerable.Empty<string>().AsResult()
                : File.ReadLines(filename).AsResult();

        private static Result<IImageSaveService> GetSaveServiceFromName(Options options)
        {
            return options.Extension switch
            {
                "png"  => new PngSaveService(),
                "jpeg" => new JpegSaveService(),
                "bmp"  => new BmpSaveService(),
                _      => Result.Fail<IImageSaveService>($"Cannot save file with {options.Extension} extension")
            };
        }

        private static Result<ILayouter> GetLayouter(Options options)
        {
            return options.Algorithm switch
            {
                "circular" => new NonIntersectedLayouter(Point.Empty, new CircularVectorsGenerator(0.005, 360)),
                "random"   => CreateRandomLayouter(options).Then(Result.Ok<ILayouter>),
                _          => Result.Fail<ILayouter>($"Layouter {options.Algorithm} not defined")
            };
        }

        private static Result<NonIntersectedLayouter> CreateRandomLayouter(Options options)
        {
            return
                from size in GetSizeRange(options)
                from vectorsGenerator in RandomVectorsGenerator.Create(new Random(), size)
                select new NonIntersectedLayouter(Point.Empty, vectorsGenerator);
        }

        private static Result<PositiveSize> GetSizeRange(Options options)
        {
            var size = Size.Round(new SizeF(options.Width * 0.5f, options.Height * 0.5f));
            return PositiveSize.Create(size.Width, size.Height);
        }

        private static Result<ITagColorGenerator> GetTagsColorGeneratorFromName(string name)
        {
            return name switch
            {
                "random"  => new RandomTagColorGenerator(new RandomColorGenerator(new Random())),
                "rainbow" => new RandomTagColorGenerator(new RainbowColorGenerator(new Random())),
                _ => Enum.TryParse(name, true, out KnownColor color)
                    ? new StrengthAlphaTagColorGenerator(Color.FromKnownColor(color))
                    : Result.Fail<ITagColorGenerator>($"Color {name} not defined")
            };
        }

        private static Result<IWordsProvider> GetWordsFromFileProviderForFile(string pathToFile)
        {
            return Result.Ok()
                .ValidateNonNull(pathToFile, nameof(pathToFile))
                .Then(() =>
                {
                    var extension = Path.GetExtension(pathToFile)[1..];
                    return extension switch
                    {
                        "txt"  => new WordsFromTxtFileProvider(pathToFile),
                        "doc"  => new WordsFromDocFileProvider(pathToFile),
                        "docx" => new WordsFromDocFileProvider(pathToFile),
                        "pdf"  => new WordsFromPdfFileProvider(pathToFile),
                        _      => Result.Fail<IWordsProvider>($"Cannot find file reader for *.{extension} not found")
                    };
                });
        }
    }
}