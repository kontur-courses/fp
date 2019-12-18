using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudContainer.Api;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Implementations
{
    [CliElement("circular")]
    public class CircularCloudLayouter : ICloudLayouter
    {
        private float spiralCounter;

        public Result<Rectangle> PutNextRectangle(Size rectangleSize, List<Rectangle> container)
        {
            return GetNextEmptyRectangleAtSpiral(rectangleSize, container)
                .Then(r =>
                {
                    container.Add(r);
                    return r;
                });
        }

        private Result<Rectangle> GetNextEmptyRectangleAtSpiral(Size rectangleSize, List<Rectangle> container)
        {
            return Result.Ok(container)
                .Then(c => c ?? new List<Rectangle>())
                .Then(c => GetPointsOnSpiral()
                    .Select(p => new Rectangle(p - rectangleSize / 2, rectangleSize))
                    .SkipWhile(r => container.Any(r.IntersectsWith))
                    .FirstOrDefault());
        }

        private IEnumerable<Point> GetPointsOnSpiral()
        {
            while (true)
            {
                var distanceFromCenter = MathF.Sqrt(spiralCounter);
                var x = (int) (distanceFromCenter * MathF.Cos(spiralCounter));
                var y = (int) (distanceFromCenter * MathF.Sin(spiralCounter++));
                yield return new Point(x, y);
            }
        }
    }
}