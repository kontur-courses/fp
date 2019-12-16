using System.Drawing;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;
using FailuresProcessing;
using System;
using TagsCloudGenerator.DTO;

namespace TagsCloudGenerator.Painters
{
    [Factorial("PainterWithUserColors")]
    public class PainterWithUserColors : IPainter
    {
        private readonly IPainterSettings painterSettings;

        public PainterWithUserColors(IPainterSettings painterSettings) =>
            this.painterSettings = painterSettings;

        public Result<None> DrawWords(WordDrawingDTO[] layoutedWords, Graphics graphics)
        {
            GetNextColor(reset: true);
            return 
                CheckUserColors()
                .Then(none =>
                {
                    graphics.Clear(painterSettings.BackgroundColor);
                    using (var solidBrush = new SolidBrush(Color.Black))
                        foreach (var wordDrawing in layoutedWords)
                        {
                            solidBrush.Color = GetNextColor();
                            var drawingResult =
                                Result.Ok(new Font(wordDrawing.FontName, wordDrawing.MaxFontSymbolWidth))
                                .Then(font => CheckFont(font, wordDrawing.FontName))
                                .ThenWithDisposableObject(font =>
                                {
                                    graphics.DrawString(wordDrawing.Word, font, solidBrush, wordDrawing.WordRectangle);
                                    return Result.Ok();
                                });
                            if (!drawingResult.IsSuccess)
                                return drawingResult;
                        }
                    return Result.Ok();
                })
                .RefineError($"{nameof(PainterWithUserColors)} failure");
        }

        private Color GetNextColor(bool reset = false) => 
            painterSettings.Colors[Math.Abs(GetNextIndex(reset)) % painterSettings.Colors.Length];

        private int colorsCounter = 0;
        private int GetNextIndex(bool reset = false) => reset ? colorsCounter = 0 : unchecked(++colorsCounter);

        private Result<Font> CheckFont(Font font, string userFontName) =>
            font.Name.ToLower() == userFontName.ToLower() ?
            Result.Ok(font) :
            Result.Fail<Font>($"Font with name \'{userFontName}\' not found in system");

        private Result<None> CheckUserColors() =>
            painterSettings.Colors.Length > 0 ?
            Result.Ok() :
            Result.Fail<None>("Number of colors must be greater than 0");
    }
}