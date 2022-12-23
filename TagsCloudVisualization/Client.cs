using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ResultOf;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.TextFormatters;

namespace TagsCloudVisualization
{
    public class Client
    {
        private readonly ITextFormatter formatter;
        private readonly ICloudLayouter layouter;
        private readonly IPainter painter;

        public Client(ICloudLayouter layouter, IPainter painter, ITextFormatter formatter)
        {
            this.layouter = layouter;
            this.painter = painter;
            this.formatter = formatter;
        }

        public Result<None> Draw(string destinationPath, FontFamily font)
        {
            var services = Program.Container.GetServices<IFileReader>();
            foreach (var service in services)
                if (service.TryReadAllText(out var text))
                    return formatter.Format(text)
                        .Then(t => MakeRectangles(t, font)
                            .Then(t => painter.DrawWordsToFile(t, destinationPath)));

            return Result.Fail<None>("IFileReader not found!");
        }


        private Result<List<Word>> MakeRectangles(List<Word> words, FontFamily fontFamily)
        {
            var processedWords = new List<Word>();

            double minFrequency = words.Min(x => x.Frequency);

            double maxFrequency = words.Max(x => x.Frequency);

            if (Math.Abs(maxFrequency - minFrequency) < 1e-6)
                return Result.Fail<List<Word>>("at least one word must pass the filter");

            foreach (var word in words)
            {
                var resOfFont =
                    TryGetWordFontByFrequency(fontFamily, 12, 36, minFrequency, maxFrequency, word.Frequency)
                        .Then(t =>
                        {
                            word.Font = t;
                            return GetWordLayoutRectangleSize(word.Value, t);
                        })
                        .Then(t => layouter.PutNextRectangle(t))
                        .Then(t =>
                        {
                            word.Rectangle = t;
                            processedWords.Add(word);
                        });
                if (!resOfFont.IsSuccess)
                    return Result.Fail<List<Word>>(resOfFont.Error);
            }

            return processedWords;
        }

        private Result<Font> TryGetWordFontByFrequency(FontFamily fontFamily, int minFontSize, int maxFontSize,
            double minFrequency, double maxFrequency, double wordFrequency)
        {
            return Result.Of(() =>
            {
                var fontSize = (int)(minFontSize + (maxFontSize - minFontSize) * (wordFrequency - minFrequency) /
                    (maxFrequency - minFrequency));
                return new Font(fontFamily, fontSize);
            });
        }

        private Result<Size> GetWordLayoutRectangleSize(string word, Font font)
        {
            return Result.Of(() => new Bitmap(1, 1))
                .Then(t => Graphics.FromImage(t))
                .Then(t => t.MeasureString(word, font))
                .Then(t =>
                {
                    var width = (int)Math.Ceiling(t.Width);
                    var height = (int)Math.Ceiling(t.Height);
                    return new Size(width, height);
                });
        }
    }
}