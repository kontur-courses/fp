using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace TagsCloudVisualizationDI.TextAnalyze.Visualization
{
    public class DefaultVisualization : IVisualization
    {
        private Brush ColorBrush { get; }
        private Font TextFont { get; }
        private Size ImageSize { get; }

        private int SizeMultiplier { get; }

        public DefaultVisualization(Brush brush, Font font, Size imageSize, int sizeMultiplier)
        {
            Result.OnFalse(CheckFont(font), er => Program.PrintAboutFail(er), $"Incorrect font name {font.Name}");

            ColorBrush = brush;
            TextFont = font;
            ImageSize = imageSize;
            SizeMultiplier = sizeMultiplier;
        }

        private static bool CheckFont(Font font)
        {
            return FontFamily.Families.Select(family => family.Name).Contains(font.Name);
        }

        public Result<None> DrawAndSaveImage(List<RectangleWithWord> elements, string savePath, ImageFormat format)
        {
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
            TextFont.Dispose();
        }

        public List<RectangleWithWord> FindSizeForElements(Dictionary<string, RectangleWithWord> formedElements)
        {
            var result = new List<RectangleWithWord>();

            foreach (var element in formedElements.Values)
            {
                using var image = new Bitmap(ImageSize.Width, ImageSize.Height);
                using var graphics = Graphics.FromImage(image);


                var fontSize = element.WordElement.CntOfWords * SizeMultiplier;
                var font = new Font("Times", fontSize);


                var newSize = graphics.MeasureString(element.WordElement.WordText, font);
                var sizedElement = new RectangleWithWord(new Rectangle
                        (element.RectangleElement.Location, newSize.ToSize()),
                    element.WordElement);
                result.Add(sizedElement);
            }

            return result;
        }
    }
}
