using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using ResultOf;
using TagsCloudContainer.Core.ImageBuilder;
using TagsCloudContainer.Core.ImageSavers;
using TagsCloudContainer.Core.LayoutAlgorithms;
using TagsCloudContainer.Core.Readers;
using TagsCloudContainer.Core.TextHandler.WordConverters;
using TagsCloudContainer.Core.TextHandler.WordFilters;
using TagsCloudContainer.Core.UserInterfaces.ConsoleUI;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer.Core
{
    class ApplicationCore
    {
        private readonly ReaderFinder readerFinder;
        private readonly IImageBuilder tagCloudImageBuilder;
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly IImageSaver imageSaver;
        private readonly Filter filter;
        private readonly WordConverter wordConverter;

        public ApplicationCore(ReaderFinder readerFinder,
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

        public void Run(Options options)
        {
            FormBoringWords(options.FileWithBoringWords, readerFinder, filter)
                .Then(words => filter.UserExcludedWords = words)
                .OnFail(HandleError);
            var rightWords = FormWords(options.InputFile, readerFinder, filter)
                .Then(words => words.MostCommon(30)
                    .Select(kvp => kvp.Element)
                    .ToArray())
                .OnFail(HandleError);
            GetBaseFont(options.FontName)
                .Then(font => FormTags(rightWords.Value, font))
                .Then(tags => tagCloudImageBuilder.Build(tags, layoutAlgorithm.GetLayoutSize()))
                .Then(bitmap => imageSaver.Save(options.OutputFile, bitmap, options.ImageFormat))
                .OnFail(HandleError);
        }

        private void HandleError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        private Result<HashSet<string>> FormWords(string path, ReaderFinder readerFinder, Filter filter)
        {
            return ReadWords(path, readerFinder)
                .Then(filter.FilterWords)
                .Then(wordConverter.ConvertWords)
                .Then(words => words.ToHashSet());
        }

        private Result<HashSet<string>> ReadWords(string path, ReaderFinder readerFinder)
        {
            return File.Exists(path)
                .AsResult()
                .FailIf(e => !e, $"Файла {path} не существует")
                .Then(e => readerFinder.Find(path))
                .Then(reader => reader.ReadWords(path))
                .Then(words => words.ToHashSet())
                .OnFail(HandleError);
        }

        private Result<Font> GetBaseFont(string fontName)
        {
            return new Font(fontName, 1)
                .AsResult()
                .FailIf(f => f.Name != fontName, $"Шрифт {fontName} не найден");
        }

        private Result<HashSet<string>> FormBoringWords(string path, ReaderFinder readerFinder, Filter filter)
        {
            return path == null
                ? Result.Ok(new HashSet<string>())
                : FormWords(path, readerFinder, filter);
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