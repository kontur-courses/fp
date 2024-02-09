using System.Drawing;
using TagCloudResult.TextProcessing;

namespace TagCloudResult.Layouter
{
    public class RectanglesGenerator(ITextProcessor textProcessor, Settings settings, ILayouter layouter)
        : IRectanglesGenerator
    {
        public Result<IEnumerable<RectangleData>> GetRectanglesData()
        {
            var frequencies = textProcessor.GetWordsFrequency();
            if (!frequencies.IsSuccess)
                return frequencies.Fail<IEnumerable<RectangleData>>();

            var fontResult = Result.Of(() => new Font(settings.FontName, settings.FontSize, FontStyle.Regular))
                .ReplaceError(x => $"Font with name {settings.FontName} does not exist or not installed");
            if (!fontResult.IsSuccess)
                return fontResult.Fail<IEnumerable<RectangleData>>();

            var totalAmount = frequencies.Value.Sum(x => x.Value);
            var result = new List<RectangleData>();
            foreach (var frequency in frequencies.Value.OrderByDescending(x => x.Value))
            {
                using var font = new Font(
                    settings.FontName, settings.FontSize * (frequency.Value * 100 / totalAmount), FontStyle.Regular
                );
                var rectangleResult = layouter.PutNextRectangle(GetTextSize(frequency.Key, font));
                if (!rectangleResult.IsSuccess)
                    return rectangleResult.Fail<IEnumerable<RectangleData>>();

                result.Add(new RectangleData(rectangleResult.Value, frequency.Key, font.Size));
            }

            return result;
        }

        private static Size GetTextSize(string text, Font font)
        {
            using var temporaryBitmap = new Bitmap(1, 1);
            using var temporaryGraphics = Graphics.FromImage(temporaryBitmap);
            return Size.Ceiling(temporaryGraphics.MeasureString(text, font));
        }
    }
}
