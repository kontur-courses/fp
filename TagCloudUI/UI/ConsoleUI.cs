using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagCloud.Core;
using TagCloud.Core.ColoringAlgorithms;
using TagCloud.Core.FileReaders;
using TagCloud.Core.ImageCreators;
using TagCloud.Core.ImageSavers;
using TagCloud.Core.LayoutAlgorithms;
using TagCloud.Core.WordsProcessors;
using TagCloudUI.Infrastructure;
using TagCloudUI.Infrastructure.Selectors;

namespace TagCloudUI.UI
{
    public class ConsoleUI : IUserInterface
    {
        private readonly IReaderSelector readersSelector;
        private readonly IWordsProcessor wordsProcessor;
        private readonly ILayoutAlgorithmSelector layoutAlgorithmSelector;
        private readonly IImageCreator imageCreator;
        private readonly IColoringAlgorithmSelector coloringAlgorithmSelector;
        private readonly IImageSaver imageSaver;

        public ConsoleUI(IReaderSelector readersSelector,
            IWordsProcessor wordsProcessor,
            ILayoutAlgorithmSelector layoutAlgorithmSelector,
            IImageCreator imageCreator,
            IColoringAlgorithmSelector coloringAlgorithmSelector,
            IImageSaver imageSaver)
        {
            this.readersSelector = readersSelector;
            this.wordsProcessor = wordsProcessor;
            this.layoutAlgorithmSelector = layoutAlgorithmSelector;
            this.imageCreator = imageCreator;
            this.coloringAlgorithmSelector = coloringAlgorithmSelector;
            this.imageSaver = imageSaver;
        }

        public void Run(IAppSettings settings)
        {
            GetCloudImage(settings)
                .Then(bitmap => imageSaver.Save(bitmap, settings.OutputPath, settings.ImageFormat))
                .Then(savedPath => Console.WriteLine($"Tag cloud visualization saved to: {savedPath}"))
                .OnFail(PrintErrorAndExit);
        }

        private static void PrintErrorAndExit(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        private Result<Bitmap> GetCloudImage(IAppSettings settings)
        {
            return GetWordsFromFile(settings.InputPath)
                .Then(words => ProcessWords(words, settings.WordsCount))
                .Then(processedWords => CreateLayout(processedWords, settings.LayoutAlgorithmType, settings.FontName))
                .FailIf(layoutInfo => IsSmallSizeForLayout(settings.ImageWidth, settings.ImageHeight, layoutInfo.Size),
                    $"Unable to place TagCloud to this image size: {settings.ImageWidth}x{settings.ImageHeight}")
                .Then(layoutInfo => CreateImage(layoutInfo, settings.ColoringTheme, settings.FontName));
        }

        private Result<Bitmap> CreateImage(LayoutInfo layoutInfo,
            ColoringTheme theme, string fontName)
        {
            return coloringAlgorithmSelector.GetAlgorithm(theme)
                .Then(algorithm => imageCreator.Create(algorithm, layoutInfo.Tags, fontName, layoutInfo.Size));
        }

        private static bool IsSmallSizeForLayout(int width, int height, Size layoutSize)
        {
            return width < layoutSize.Width || height < layoutSize.Height;
        }

        private Result<IEnumerable<string>> GetWordsFromFile(string inputPath)
        {
            return Path.GetExtension(inputPath).TrimStart('.').AsResult()
                .Then(ParseFileExtension)
                .Then(extension => readersSelector.GetReader(extension))
                .Then(reader => reader.ReadAllWords(inputPath));
        }

        private static Result<FileExtension> ParseFileExtension(string extension)
        {
            return Enum.TryParse(extension, true, out FileExtension result)
                ? result.AsResult()
                : Result.Fail<FileExtension>($"Unable to read file with this extension: {extension}");
        }

        private Result<List<string>> ProcessWords(IEnumerable<string> words,
            int wordsCount)
        {
            return wordsProcessor.Process(words, wordsCount)
                .ToList()
                .AsResult();
        }

        private Result<LayoutInfo> CreateLayout(IReadOnlyCollection<string> words,
            LayoutAlgorithmType algorithmType, string fontName)
        {
            return layoutAlgorithmSelector.GetAlgorithm(algorithmType)
                .Then(algorithm =>
                {
                    return words.Select((word, index) => CreateTag(algorithm, word, index, words.Count, fontName))
                        .ToList()
                        .AsResult()
                        .Then(tags => new LayoutInfo(tags, algorithm.GetLayoutSize()));
                });
        }

        private static Tag CreateTag(
            ILayoutAlgorithm algorithm, string word, int index, int wordsCount, string fontName)
        {
            var fontSize = wordsCount + 10 - index;
            using var font = new Font(fontName, fontSize);
            var tagSize = TextRenderer.MeasureText(word, font);

            return new Tag(word, algorithm.PutNextRectangle(tagSize), fontSize);
        }
    }
}