using TagsCloudContainer.Infrastucture;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Algorithm
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly ImageSettings imageSettings;
        private readonly IRectanglePlacer rectanglePlacer;
        private readonly ICloudSizer cloudSizer;


        public CircularCloudLayouter(ImageSettings imageSettings, IRectanglePlacer rectanglePlacer, ICloudSizer cloudSizer)
        {
            this.imageSettings = imageSettings;
            this.rectanglePlacer = rectanglePlacer;
            this.cloudSizer = cloudSizer;
        }

        public Result<List<TextRectangle>> GetRectangles(Dictionary<string, int> wordFrequencies)
        {
            var rectangles = new List<TextRectangle>();

            foreach (var wordFrequency in wordFrequencies.OrderByDescending(x => x.Value))
            {
                var word = wordFrequency.Key;
                var cloudSize = cloudSizer.GetCloudSize(wordFrequencies, word);
                var rectangle = rectanglePlacer.GetPossibleNextRectangle(rectangles, cloudSize.Item2);

                if (!rectangle.IsSuccess)
                    return Result.Fail<List<TextRectangle>>(rectangle.Error);

                rectangles.Add(new TextRectangle(rectangle.Value, word, cloudSize.Item1));
            }

            return rectangles.All(textRect => textRect.FitsIntoImage(imageSettings.Width, imageSettings.Height)) ?
                Result.Ok(rectangles) : Result.Fail<List<TextRectangle>>("The tag cloud goes beyond the boundaries of the image");
        }
    }
}
