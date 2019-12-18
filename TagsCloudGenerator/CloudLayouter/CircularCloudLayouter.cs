using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudGenerator.Tools;
using TagsCloudGenerator.Visualizer;

namespace TagsCloudGenerator.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Cloud cloud;
        private readonly IEnumerator<Point> layoutPointsEnumerator;
        private readonly ImageSettings imageSettings;

        public CircularCloudLayouter(Point center, ILayoutPointsGenerator layoutPointsGenerator, ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;
            layoutPointsEnumerator = layoutPointsGenerator.GetPoints().GetEnumerator();

            cloud = new Cloud(center);
        }

        public Result<Cloud> LayoutWords(Dictionary<string, int> wordToCount)
        {
            return Result
                .Of(() => GetWords(wordToCount))
                .Then(words => cloud.AddWords(words))
                .RefineError("Couldn't layout words");
        }

        private IEnumerable<Word> GetWords(Dictionary<string, int> wordToCount)
        {
            return wordToCount
                .OrderByDescending(x => x.Value)
                .Select(x => (word: x, size: GetSizeOfWord(x.Key, x.Value)))
                .Select(p => GetNextWord(p.word.Key, p.word.Value, p.size));
        }

        private Size GetSizeOfWord(string word, int count)
        {
            return TextRenderer.MeasureText(word,
                new Font(imageSettings.Font.FontFamily, imageSettings.Font.Size * count));
        }

        private Word GetNextWord(string value, int count, Size rectangleSize)
        {
            var position = GetRectangleLocation(rectangleSize);
            var rectangle = new Rectangle(position, rectangleSize);
            var word = new Word(value, rectangle, count);

            return word;
        }

        private Point GetRectangleLocation(Size rectangleSize)
        {
            (bool success, Point location) nextLocation;

            do
            {
                nextLocation = TryGetNextLocation(rectangleSize);
            } while (!nextLocation.success);

            var location = nextLocation.location;

            if (cloud.Words.Count == 0)
                return location;

            location = TryMove(rectangleSize, location, cloud.Center);
            var previous = cloud.Words.Last().Rectangle.Location;

            return TryMove(rectangleSize, location, previous);
        }

        private (bool success, Point location) TryGetNextLocation(Size rectangleSize)
        {
            layoutPointsEnumerator.MoveNext();
            var center = layoutPointsEnumerator.Current;

            var upperLeftCorner = GetUpperLeftCornerPosition(rectangleSize, center);
            var rectangle = new Rectangle(upperLeftCorner, rectangleSize);

            return (NotIntersectsWithOther(rectangle), upperLeftCorner);
        }

        private Point TryMove(Size rectangleSize, Point from, Point to)
        {
            var newLocation = from;
            var nearestToTarget = to;
            var minDistance = Math.Sqrt(2);

            while (newLocation.Distance(nearestToTarget) > minDistance)
            {
                var middle = newLocation.GetMiddlePoint(nearestToTarget);
                var rectangle = new Rectangle(middle, rectangleSize);

                if (NotIntersectsWithOther(rectangle))
                    newLocation = middle;
                else
                    nearestToTarget = middle;
            }

            return newLocation;
        }

        private bool NotIntersectsWithOther(Rectangle rectangle)
        {
            return cloud.Words.All(r => !r.Rectangle.IntersectsWith(rectangle));
        }

        protected Point GetUpperLeftCornerPosition(Size rectangleSize, Point center)
        {
            var xOffset = rectangleSize.Width / 2;
            var yOffset = rectangleSize.Height / 2;

            return new Point(center.X - xOffset, center.Y - yOffset);
        }
    }
}