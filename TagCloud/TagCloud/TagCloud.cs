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
        private readonly TextWriter _statusWriter;
        private readonly ITextProcessor _textProcessor;
        private readonly IWordLayouter _wordLayouter;
        private List<Dictionary<string, int>> _processedTexts = new();

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
            if (!_processedTexts.Any())
            {
                _statusWriter.WriteLine("Сначала нужно подготовить данные. Сейчас список обработанных текстов пуст.");
                return this;
            }

            foreach (var text in _processedTexts)
            {
                _statusWriter.WriteLine("Раскладываю текст");
                var layoutedWords = _wordLayouter.Layout(options, text);
                _statusWriter.WriteLine("Рисую bitmap");
                var bitmap = _drawer.Draw(options, layoutedWords);
                if (options.Size != Size.Empty)
                {
                    _statusWriter.WriteLine("Масштабирую к заданному размеру. (Возможно ухудшение качества)");
                    bitmap = bitmap.ScaledResize(options.Size, options.BackgroundColor);
                }

                _statusWriter.WriteLine("Сохраняю bitmap\n");
                TagCloudImageSaver.Save(bitmap, options.FileName, options.Path, options.Format);
            }

            _statusWriter.WriteLine($"Готово!\nФайлы здесь: {options.Path}");
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