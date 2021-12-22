using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace TagsCloudVisualizationDI.TextAnalyze.Visualization
{
    public class DefaultVisualization : IVisualization
    {
        private Brush ColorBrush { get; }

        private Size ImageSize { get; }

        private Font TextFont { get; }

        private int SizeMultiplier { get; }

        public DefaultVisualization(Font textFont, Brush brush, Size imageSize, int sizeMultiplier)
        {
            TextFont = textFont;
            ColorBrush = brush;
            ImageSize = imageSize;
            SizeMultiplier = sizeMultiplier;
        }

        public Result<None> DrawAndSaveImage(List<RectangleWithWord> elements, Result<string> savePathResult, ImageFormat format)
        {
            if (!savePathResult.IsSuccess)
                return Result.Fail<None>(savePathResult.Error);

            var savePath = savePathResult.Value;
            using var image = new Bitmap(ImageSize.Width, ImageSize.Height);
            var drawImage = DrawRectangles(image, elements);

            return Result.OfAction(() => drawImage.Save(savePath, format), $"Error during saving");
            
        }


        private Bitmap DrawRectangles(Bitmap image, List<RectangleWithWord> elementList)
        {
            using var graphics = Graphics.FromImage(image);
            foreach (var element in elementList)
            {
                var fontSize = SizeMultiplier * element.WordElement.CntOfWords;
                var font = new Font(TextFont.Name, fontSize);

                graphics.DrawString(element.WordElement.WordText, font, ColorBrush,
                    element.RectangleElement.Location.X, element.RectangleElement.Location.Y);
            }

            return image;
        }

        public void Dispose()
        {
            ColorBrush.Dispose();
        }

        public Result<List<RectangleWithWord>> FindSizeForElements(Dictionary<string, RectangleWithWord> formedElements)
        {
            if (!CheckFont(TextFont.FontFamily))
                return Result.Fail<List<RectangleWithWord>>($"can't find current font : {TextFont.Name}");
            var resultList = new List<RectangleWithWord>();

            foreach (var element in formedElements.Values)
            {
                using var image = new Bitmap(ImageSize.Width, ImageSize.Height);
                using var graphics = Graphics.FromImage(image);


                var fontSize = element.WordElement.CntOfWords * SizeMultiplier;
                var newFont = new Font(TextFont.Name, fontSize);


                var newSize = graphics.MeasureString(element.WordElement.WordText, newFont);
                var sizedElement = new RectangleWithWord(new Rectangle
                        (element.RectangleElement.Location, newSize.ToSize()),
                    element.WordElement);
                resultList.Add(sizedElement);
            }

            return resultList
                .OrderByDescending(el => el.WordElement.CntOfWords)
                .ToList().AsResult();
        }

        private static bool CheckFont(FontFamily fontFamily)
        {
            return FontFamily.Families.Contains(fontFamily);
        }
    }
}
