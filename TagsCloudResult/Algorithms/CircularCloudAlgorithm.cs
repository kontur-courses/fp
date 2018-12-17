using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using TagsCloudResult.Settings;

namespace TagsCloudResult.Algorithms
{
    public class CircularCloudAlgorithm : IAlgorithm
    {
        private readonly List<Rectangle> rectangles;
        private readonly ICloudSettings cloudSettings;
        private readonly ISpiral spiral;

        public CircularCloudAlgorithm(ICloudSettings cloudSettings, ISpiral spiral)
        {

            rectangles = new List<Rectangle>();
            this.cloudSettings = cloudSettings;
            this.spiral = spiral;
        }

        public Result<IReadOnlyCollection<Tag>> GenerateTags(IReadOnlyDictionary<string, int> processedWords)
        {
            return Result.Of(() =>
            {
                var result = new Collection<Tag>();

                var relativeWeightsSum = processedWords.Sum(e => e.Value);

                var relativeWeightUnit = (double)relativeWeightsSum / cloudSettings.CenterPoint.X * 2;

                foreach (var word in processedWords)
                {
                    var wordWeight = word.Value * relativeWeightUnit;
                    var size = new Size((int)wordWeight, (int)(wordWeight / 2));

                    var newRectangle = PutNextRectangle(size);

                    result.Add(new Tag { Text = word.Key, Rectangle = newRectangle });
                }
                return (IReadOnlyCollection<Tag>)result;
            });
        }

        public ReadOnlyCollection<Rectangle> GetRectangles() => rectangles.AsReadOnly();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 1 || rectangleSize.Width < 1)
                throw new ArgumentException("Размер прямоугольника должен быть больше 0");

            if (!rectangles.Any())
            {
                var firstRectangleLocation
                    = new Point(cloudSettings.CenterPoint.X - rectangleSize.Width / 2, cloudSettings.CenterPoint.Y - rectangleSize.Height / 2);
                var firstRectangle = new Rectangle(firstRectangleLocation, rectangleSize);
                rectangles.Add(firstRectangle);
                return firstRectangle;
            }
            var rectangle = CreateNewRectangle(rectangleSize);

            while (rectangles.Any(e => e.IntersectsWith(rectangle)))
            {
                rectangle = CreateNewRectangle(rectangleSize);
            }
            rectangle = MoveRectangleCloserToCenter(rectangle);
            rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetRectangleCenter(Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                rect.Top + rect.Height / 2);
        }
        private Rectangle MoveRectangleCloserToCenter(Rectangle rectangle)
        {
            rectangle = MoveRectangleCloserToCenterByY(rectangle);
            return MoveRectangleCloserToCenterByX(rectangle);
        }
        private Rectangle MoveRectangleCloserToCenterByY(Rectangle rectangle)
        {
            while (!(GetRectangleCenter(rectangle).Y == cloudSettings.CenterPoint.Y ||
                     rectangles.Any(e => e.IntersectsWith(rectangle))))
            {
                if (GetRectangleCenter(rectangle).Y > cloudSettings.CenterPoint.Y)
                    rectangle.Y--;

                if (GetRectangleCenter(rectangle).Y < cloudSettings.CenterPoint.Y)
                    rectangle.Y++;
            }
            return rectangle;

        }
        private Rectangle MoveRectangleCloserToCenterByX(Rectangle rectangle)
        {
            while (!(GetRectangleCenter(rectangle).X == cloudSettings.CenterPoint.X ||
                     rectangles.Any(e => e.IntersectsWith(rectangle))))
            {
                if (GetRectangleCenter(rectangle).X > cloudSettings.CenterPoint.X)
                    rectangle.X--;

                if (GetRectangleCenter(rectangle).X < cloudSettings.CenterPoint.X)
                    rectangle.X++;
            }
            return rectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
        {
            var rectangleLocation = spiral.GetNextPoint();
            var newRectangle = new Rectangle(rectangleLocation.Value, rectangleSize);
            return newRectangle;
        }
    }
}
