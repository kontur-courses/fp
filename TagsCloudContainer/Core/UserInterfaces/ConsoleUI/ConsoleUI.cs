using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using ResultOf;
using TagsCloudContainer.Core.Extensions;
using TagsCloudContainer.Core.ImageBuilder;
using TagsCloudContainer.Core.ImageSavers;
using TagsCloudContainer.Core.LayoutAlgorithms;
using TagsCloudContainer.Core.Readers;
using TagsCloudContainer.Core.TextHandler.WordConverters;
using TagsCloudContainer.Core.TextHandler.WordFilters;

namespace TagsCloudContainer.Core.UserInterfaces.ConsoleUI
{
    class ConsoleUi : IUi
    {
        private readonly ReaderFinder readerFinder;
        private readonly IImageBuilder tagCloudImageBuilder;
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly IImageSaver imageSaver;
        private readonly Filter filter;
        private readonly WordConverter wordConverter;

        public ConsoleUi(ReaderFinder readerFinder,
            IImageBuilder tagCloudImageBuilder, ILayoutAlgorithm layoutAlgorithm,
            IImageSaver imageSaver,
            Filter filter, WordConverter wordConverter)
        {
            this.filter = filter;
            this.wordConverter = wordConverter;
            this.readerFinder = readerFinder;
            this.tagCloudImageBuilder = tagCloudImageBuilder;
            this.imageSaver = imageSaver;
            this.layoutAlgorithm = layoutAlgorithm;
        }

        public void Run(IEnumerable<string> userInput)
        {
            Parser.Default
                .ParseArguments<Options>(userInput)
                .WithParsed(Run);
        }

        private void HandleError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        private void Run(Options options)
        {
            FormBoringWords(options.FileWithBoringWords)
                .Then(words => filter.UserExcludedWords = words)
                .OnFail(HandleError);
            var rightWords = FormWordsFromFile(options.InputFile)
                .Then(words => words.MostCommon(30)
                    .Select(kvp => kvp.Item1)
                    .ToArray())
                .OnFail(HandleError);
            GetBaseFont(options.FontName)
                .Then(font => FormTags(rightWords.Value, font))
                .Then(tags => tagCloudImageBuilder.Build(tags, layoutAlgorithm.GetLayoutSize()))
                .Then(bitmap => imageSaver.Save(options.OutputFile, bitmap, options.ImageFormat))
                .OnFail(HandleError);
        }

        private Result<HashSet<string>> FormWordsFromFile(string path)
        {
            var reader = readerFinder.Find(path);
            if (reader == null)
                return Result.Fail<HashSet<string>>("Формат входного файла не поддерживается");
            return reader.ReadWords(path)
                .Then(filter.FilterWords)
                .Then(wordConverter.ConvertWords)
                .Then(words => words.ToHashSet());
        }

        private Result<Font> GetBaseFont(string fontName)
        {
            var font = new Font(fontName, 1);
            return font.Name == fontName
                ? Result.Ok(font)
                : Result.Fail<Font>($"Шрифт {fontName} не найден");
        }

        private Result<HashSet<string>> FormBoringWords(string path)
        {
            return path == null
                ? Result.Ok(new HashSet<string>())
                : FormWordsFromFile(path);
        }

        private IEnumerable<Tag> FormTags(IReadOnlyList<string> words, Font baseFont)
        {
            var tags = new List<Tag>();
            for (var i = 0; i < words.Count; i++)
            {
                var word = words[i];
                var font = new Font(baseFont.Name, 40 - i);
                var size = TextRenderer.MeasureText(word, font);
                tags.Add(new Tag(word, layoutAlgorithm.PutNextRectangle(size), font));
            }

            return tags;
        }
    }
}