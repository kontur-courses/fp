using TagsCloudContainer.Infrastucture;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Algorithm
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly ImageSettings imageSettings;
        private readonly IRectanglePlacer rectanglePlacer;


        public CircularCloudLayouter(ImageSettings imageSettings, IRectanglePlacer rectanglePlacer)
        {
            this.imageSettings = imageSettings;
            this.rectanglePlacer = rectanglePlacer;
        }

        public Result<List<TextRectangle>> GetRectangles(Dictionary<string, int> wordFrequencies)
        {
            var rectangles = new List<TextRectangle>();
            var bitmap = new Bitmap(imageSettings.Width, imageSettings.Height);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var word in wordFrequencies.Keys)
            {
                var fontSize = CalculateFontSize(wordFrequencies, word, imageSettings.Font.Size);
                var font = new Font(imageSettings.Font.FontFamily, fontSize, imageSettings.Font.Style, imageSettings.Font.Unit);
                var textSize = graphics.MeasureString(word, font);
                var rectangle = rectanglePlacer.GetPossibleNextRectangle(rectangles, textSize);

                if (!rectangle.IsSuccess)
                    return Result.Fail<List<TextRectangle>>(rectangle.Error);

                rectangles.Add(new TextRectangle(rectangle.Value, word, font));
            }

            return rectangles.All(textRect => textRect.FitsIntoImage(imageSettings.Width, imageSettings.Height)) ?
                Result.Ok(rectangles) : Result.Fail<List<TextRectangle>>("The tag cloud goes beyond the boundaries of the image");
        }

        private float CalculateFontSize(Dictionary<string, int> wordFrequencies, string word, float fontSize)
        {
            return fontSize + (wordFrequencies[word] - wordFrequencies.Values.Min()) * 20 / (wordFrequencies.Values.Max());
        }
    }
}
