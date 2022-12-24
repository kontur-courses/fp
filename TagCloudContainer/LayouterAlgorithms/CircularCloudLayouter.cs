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

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            var rectangle = Spiral.GetPoints()
                .Select(point =>
                {
                    var coordinatesOfRectangle =
                        RectangleCoordinatesCalculator.CalculateRectangleCoordinates(point, rectangleSize);
                    return !coordinatesOfRectangle.IsSuccess ?
                        new Result<Rectangle>(coordinatesOfRectangle.Error) :
                        new Result<Rectangle>(null,new Rectangle(coordinatesOfRectangle.Value, rectangleSize));
                })
                .First(rectangle => !IntersectsWithOtherRectangles(rectangle));
            Rectangles.Add(rectangle.Value);
            return rectangle;
        }

        

        private bool IntersectsWithOtherRectangles(Result<Rectangle> rectangle)
        {
            return rectangle.IsSuccess && Rectangles.Any(r => r.IntersectsWith(rectangle.Value));
        }
    }
}