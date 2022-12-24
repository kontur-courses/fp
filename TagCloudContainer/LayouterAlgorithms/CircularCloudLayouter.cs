using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.TaskResult;

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
                    return coordinatesOfRectangle.Then(p => Result.OnSuccess(new Rectangle(p, rectangleSize)));
                })
                .First(rectangle => rectangle.Error != null || !IntersectsWithOtherRectangles(rectangle.Value));
            Rectangles.Add(rectangle.Value);
            return rectangle;
        }


        private bool IntersectsWithOtherRectangles(Rectangle rectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(rectangle));
        }
    }
}