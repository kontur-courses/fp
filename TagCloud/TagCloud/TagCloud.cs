using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ResultOf;
using TagCloud.BitmapSaving;
using TagCloud.Drawing;
using TagCloud.Extensions;
using TagCloud.PreLayout;
using TagCloud.TextProcessing;

namespace TagCloud
{
    public class TagCloud
    {
        private readonly IDrawer _drawer;
        private readonly List<Dictionary<string, int>> _processedTexts = new();
        private readonly TextWriter _statusWriter;
        private readonly ITextProcessor _textProcessor;
        private readonly IWordLayouter _wordLayouter;

        public TagCloud(ITextProcessor textProcessor, IWordLayouter wordLayouter, IDrawer drawer,
            TextWriter statusWriter)
        {
            _textProcessor = textProcessor;
            _wordLayouter = wordLayouter;
            _drawer = drawer;
            _statusWriter = statusWriter;
        }

        public TagCloud ProcessText(ITextProcessingOptions options)
        {
            foreach (var filePath in options.FilesToProcess)
            {
                _statusWriter.WriteLine($"Идет обработка текста {filePath}");
                _textProcessor.GetWordsWithFrequency(options, filePath)
                    .Then(_processedTexts.Add)
                    .Then(_ => _statusWriter.WriteLine("Обработка завершена\n"))
                    .OnFail(error => _statusWriter.WriteLine($"Произошла ошибка:\n{error}"));
            }

            return this;
        }

        public TagCloud DrawTagClouds(IDrawerOptions options)
        {
            if (_processedTexts.Any())
            {
                var results = _processedTexts.Select(text =>
                    text.AsResult()
                        .Then(t =>
                        {
                            _statusWriter.WriteLine("Раскладываю текст");
                            return _wordLayouter.Layout(options, t);
                        })
                        .Then(words =>
                        {
                            _statusWriter.WriteLine("Рисую bitmap");
                            return _drawer.Draw(options, words);
                        })
                        .Then(b =>
                        {
                            if (options.Size == Size.Empty) return b;
                            _statusWriter.WriteLine("Масштабирую к заданному размеру. (Возможно ухудшение качества)");
                            return b.ScaledResize(options.Size, options.BackgroundColor);
                        })
                        .Then(b =>
                        {
                            _statusWriter.WriteLine("Сохраняю bitmap\n");
                            TagCloudImageSaver.Save(b, options.FileName, options.Path, options.Format);
                        })
                        .OnFail(error => _statusWriter.WriteLine($"Произошла ошибка: {error}"))
                );
                if (results.All(r => !r.IsSuccess))
                    _statusWriter.WriteLine("Из-за возникших ошибок ни одно изображение не было нарисовано");
                _statusWriter.WriteLine($"Готово!\nФайлы здесь: {options.Path}");
                return this;
            }

            _statusWriter.WriteLine("Сначала нужно подготовить данные. Сейчас список обработанных текстов пуст.");
            return this;
        }

        public TagCloud ClearProcessedTexts()
        {
            _processedTexts.Clear();
            _statusWriter.WriteLine("Список обработанных текстов очищен");
            return this;
        }
    }
}