using System.Drawing;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;
using FailuresProcessing;

namespace TagsCloudGenerator.Painters
{
    [Factorial("PainterWithUserColors")]
    public class PainterWithUserColors : IPainter
    {
        private readonly IPainterSettings painterSettings;

        public PainterWithUserColors(IPainterSettings painterSettings) =>
            this.painterSettings = painterSettings;

        public Result<None> DrawWords(
            (string word, float maxFontSymbolWidth, string fontName, RectangleF wordRectangle)[] layoutedWords,
            Graphics graphics)
        {
            return 
                CheckUserColors()
                .Then(none =>
                {
                    graphics.Clear(painterSettings.BackgroundColor);
                    var count = 0;
                    var colors = painterSettings.Colors;
                    foreach (var (word, maxFontSymbolWidth, fontName, rect) in layoutedWords)
                        using (var font = new Font(fontName, maxFontSymbolWidth))
                            graphics.DrawString(
                                word,
                                font,
                                new SolidBrush(colors[count = ++count % colors.Length]),
                                rect);
                    return Result.Ok();
                })
                .RefineError($"{nameof(PainterWithUserColors)} failure");
        }

        private Result<None> CheckUserColors() =>
            painterSettings.Colors.Length > 0 ?
            Result.Ok() :
            Result.Fail<None>("Number of colors must be greater than 0");
    }
}