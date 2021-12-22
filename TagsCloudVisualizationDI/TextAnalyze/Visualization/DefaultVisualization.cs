using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace TagsCloudVisualizationDI.TextAnalyze.Visualization
{
    public class DefaultVisualization : IVisualization
    {
        private Brush ColorBrush { get; }
        //private Font TextFont { get; }
        private Size ImageSize { get; }

        private int SizeMultiplier { get; }

        public DefaultVisualization(Brush brush, Size imageSize, int sizeMultiplier)
        {
            //Result.OnFalse(CheckFont(font.FontFamily), er => Program.PrintAboutFail(er), $"Incorrect font name {font.Name}");

            ColorBrush = brush;
            //TextFont = font;
            ImageSize = imageSize;
            SizeMultiplier = sizeMultiplier;
        }

        private static bool CheckFont(FontFamily fontFamily)
        {
            return FontFamily.Families.Contains(fontFamily);
        }

        public Result<None> DrawAndSaveImage(List<RectangleWithWord> elements, Result<string> savePathResult, ImageFormat format, Font textFont)
        {
            if (!savePathResult.IsSuccess)
                return Result.Fail<None>($"incorrectSavePath : {savePathResult}");
            else
            {
                var savePath = savePathResult.Value;
                using var image = new Bitmap(ImageSize.Width, ImageSize.Height);
                var drawImage = DrawRectangles(image, elements, textFont);

                return Result.OfAction(() => drawImage.Save(savePath, format), $"Error during saving");
            }
        }


        private Bitmap DrawRectangles(Bitmap image, List<RectangleWithWord> elementList, Font textFont)
        {
            using var graphics = Graphics.FromImage(image);
            foreach (var element in elementList)
            {
                var fontSize = SizeMultiplier * element.WordElement.CntOfWords;
                var font = new Font(textFont.Name, fontSize);

                graphics.DrawString(element.WordElement.WordText, font, ColorBrush,
                    element.RectangleElement.Location.X, element.RectangleElement.Location.Y);
            }

            return image;
        }

        public void Dispose()
        {
            ColorBrush.Dispose();
            //TextFont.Dispose();
        }

        public Result<List<RectangleWithWord>> FindSizeForElements(Dictionary<string, RectangleWithWord> formedElements, Font font)
        {
            var resultList = new List<RectangleWithWord>();

            foreach (var element in formedElements.Values)
            {
                using var image = new Bitmap(ImageSize.Width, ImageSize.Height);
                using var graphics = Graphics.FromImage(image);


                var fontSize = element.WordElement.CntOfWords * SizeMultiplier;
                var newFont = new Font(font.Name, fontSize);


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
    }
}
