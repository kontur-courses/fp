using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Result;

namespace TagCloudContainer.LayouterAlgorithms
{
    /// <summary>
    /// Возвращает последовательность непересекающихся прямоугольников основанных на точках спирали. 
    /// </summary>
    public class CircularCloudLayouter : ICloudLayouterAlgorithm
    {
        public List<Rectangle> Rectangles { get; }
        private Spiral Spiral { get; }


        public CircularCloudLayouter(Spiral spiral)
        {
            Rectangles = new List<Rectangle>();
            Spiral = spiral;
        }

        private Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("Wrong size of rectangle");
            var rect = Spiral.GetPoints()
                .Select(point =>
                {
                    var coordinatesOfRectangle =
                        RectangleCoordinatesCalculator.CalculateRectangleCoordinates(point, rectangleSize);
                    return !coordinatesOfRectangle.IsSuccess ? 
                        new Result<Rectangle>(coordinatesOfRectangle.Error) :
                        new Result<Rectangle>(null, new Rectangle(coordinatesOfRectangle.Value, rectangleSize));
                })
                .First(rectangle => !IntersectsWithOtherRectangles(rectangle));
            if(rect.IsSuccess)
                Rectangles.Add(rect.Value);
            return rect;
        }

        public Result<Point> PlaceNextWord(string word, int wordCount, int coefficient)
        {
            var rectangleHeight = wordCount * coefficient * word.Length + coefficient;
            var rectangleWidth = wordCount * 2 * coefficient;
            var rectangle = PutNextRectangle(new Size(rectangleHeight, rectangleWidth));
            return !rectangle.IsSuccess ?
                new Result<Point>(rectangle.Error) : 
                new Result<Point>(null, new Point(rectangle.Value.X, rectangle.Value.Y));
        }

        private bool IntersectsWithOtherRectangles(Result<Rectangle> rectangle)
        {
            return rectangle.IsSuccess && Rectangles.Any(r => r.IntersectsWith(rectangle.Value));
        }
    }
}