using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.DefinerFontSize;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.Algorithm;
using TagsCloudVisualization.Infrastructure.Algorithm.Curves;
using TagsCloudVisualization.Infrastructure.Analyzer;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.InfrastructureUI.Painters
{
    public class CloudPainter
    {
        private readonly IAnalyzer analyzer;
        private readonly IDefinerFontSize definerFont;
        private readonly IImageHolder imageHolder;
        private readonly IPaletteSettings paletteSettings;
        private readonly IWordsProvider wordsProvider;

        public CloudPainter(IImageHolder imageHolder,
            IWordsProvider wordsProvider,
            IAnalyzer analyzer, IDefinerFontSize definerFont,
            IPaletteSettings paletteSettings)
        {
            this.wordsProvider = wordsProvider;
            this.imageHolder = imageHolder;
            this.definerFont = definerFont;
            this.analyzer = analyzer;
            this.paletteSettings = paletteSettings;
        }

        public Result<None> Paint(string path, ICurve curve)
        {
            var isInit = imageHolder.FailIfNotInitialized();
            if (!isInit.IsSuccess)
                return isInit;

            var imageSize = imageHolder.GetImageSize();
            if (!imageSize.IsSuccess)
                return Result.Fail<None>(imageSize.Error);
            var cloud = new Cloud(curve);

            var words = wordsProvider.GetWords(path);
            if (!words.IsSuccess)
                return Result.Fail<None>(words.Error);

            var analyzedWords = analyzer.CreateWordFrequenciesSequence(words.Value);
            if (!analyzedWords.IsSuccess)
                return Result.Fail<None>(analyzedWords.Error);

            var wordsWithFont = definerFont.DefineFontSize(analyzedWords.Value);
            if (!wordsWithFont.IsSuccess)
                return Result
                    .Fail<None>(wordsWithFont.Error)
                    .RefineError("попробуйте поменять кодировку");


            return Draw(wordsWithFont.Value, imageSize.Value, cloud);
        }

        private Result<None> Draw(IEnumerable<WordWithFont> words, Size imageSize, Cloud cloud)
        {
            var counter = 0;
            using var graphics = imageHolder.StartDrawing().Value;

            graphics.FillRectangle(new SolidBrush(paletteSettings.BackgroundColor), 0, 0,
                imageSize.Width, imageSize.Height);

            foreach (var word in words.OrderBy(w => w.Font.Size).Reverse())
            {
                var color = paletteSettings.GetColorAccordingSize(word.Font.Size);
                using var brush = new SolidBrush(color);
                var sizeRectangle = TextRenderer.MeasureText(word.Word, word.Font);
                sizeRectangle.Width++;
                sizeRectangle.Height++;
                var r = Result.Of(() => cloud.PutNextRectangle(sizeRectangle));

                if (!r.IsSuccess || IsOutsideImage(r.Value, imageSize))
                    return Result.Fail<None>(r.IsSuccess
                        ? string.Join(Environment.NewLine, "облако вышло за границы изображения,",
                            " попробуйте уменьшить максимальный/минимальный размер шрифта",
                            " или сделать изображение больше")
                        : r.Error);

                var drawFormat = new StringFormat { Alignment = StringAlignment.Center };
                graphics.DrawString(word.Word, word.Font, brush, r.Value, drawFormat);
                if (++counter % 10 == 0)
                    imageHolder.UpdateUi();
            }

            return Result.Ok();
        }

        private static bool IsOutsideImage(Rectangle rectangle, Size imageSize)
        {
            return rectangle.X < 0 || rectangle.Y < 0
                                   || rectangle.Right > imageSize.Width || rectangle.Bottom > imageSize.Height;
        }
    }
}