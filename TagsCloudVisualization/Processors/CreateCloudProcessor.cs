using System;
using System.Drawing;
using System.Linq;
using Autofac;
using TagsCloudVisualization.Commands;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.FileReaders;
using TagsCloudVisualization.Common.ImageWriters;
using TagsCloudVisualization.Common.Settings;
using TagsCloudVisualization.Common.TagCloudPainters;
using TagsCloudVisualization.Common.Tags;
using TagsCloudVisualization.Common.TextAnalyzers;

namespace TagsCloudVisualization.Processors
{
    public class CreateCloudProcessor : CommandProcessorBase<CreateCloudCommand>
    {
        private readonly ICanvasSettings canvasSettings;
        private readonly ITagStyleSettings tagStyleSettings;

        public CreateCloudProcessor(IFileReader fileReader, ITextAnalyzer textAnalyzer, ITagBuilder tagBuilder,
            ITagCloudPainter tagCloudPainter, IImageWriter imageWriter, ICanvasSettings canvasSettings,
            ITagStyleSettings tagStyleSettings)
            : base(fileReader, textAnalyzer, tagBuilder, tagCloudPainter, imageWriter)
        {
            this.canvasSettings = canvasSettings;
            this.tagStyleSettings = tagStyleSettings;
        }

        public override int Run(CreateCloudCommand options)
        {
            var result = Result.Of(() => canvasSettings)
                .And(canvas =>
                {
                    canvas.Width = options.Width;
                    canvas.Height = options.Height;
                    canvas.BackgroundColor = GetColorFromString(options.BackgroundColor).GetValueOrThrow();
                }, "Переданы неверные настройки холста изображения:")
                .Then(_ => tagStyleSettings)
                .And(tagStyle =>
                {
                    tagStyle.ForegroundColors =
                        options.ForegroundColors.Select(GetColorFromString).Select(r => r.GetValueOrThrow()).ToArray();
                    tagStyle.FontFamilies = options.Fonts.Select(font => font.Trim()).ToArray();
                    tagStyle.Size = options.TagSize;
                    tagStyle.SizeScatter = options.TagSizeScatter;
                }, "Переданы неверные настройки стилей:")
                .Then(_ => fileReader.ReadFile(options.InputFile))
                .Then(text => textAnalyzer.GetWordStatistics(text))
                .Then(stat => tagBuilder.GetTags(stat))
                .Then(tags => tagCloudPainter.Paint(tags))
                .Then(bitmap => imageWriter.Save(bitmap, options.OutputFile))
                .OnSuccess(_ => Console.WriteLine($"Облако тегов сгенерировано и сохранено '{options.OutputFile}'."))
                .OnFail(Console.WriteLine);

            return result.IsSuccess ? 0 : 1;
        }

        private static Result<Color> GetColorFromString(string color)
        {
            return Result.Of(() => ColorTranslator.FromHtml(color.Trim()))
                .ReplaceError(_ => $"невозможно преобразовать в цвет строку '{color}'.");
        }
    }
}