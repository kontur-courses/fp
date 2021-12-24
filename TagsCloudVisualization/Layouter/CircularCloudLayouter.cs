using System;
using System.Collections.Generic;
using System.Drawing;
using ResultMonad;
using ResultMonad.Extensions;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization.Layouter
{
    internal class CircularCloudLayouter : ILayouter
    {
        private readonly IPointGenerator generator;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(IPointGenerator generator)
        {
            this.generator = generator;
            rectangles = new List<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            return rectangleSize.AsResult()
                .Validate(size => size.Width > 0, "Rectangle width should be > 0")
                .Validate(size => size.Height > 0,  "Rectangle height should be > 0")
                .Then(GetCorrectRectangle)
                .Then(rectangle =>
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                });
        }

        private Rectangle GetCorrectRectangle(Size size)
        {
            Rectangle rectangle;
            do
            {
                var point = generator.GetNextPoint();
                var location = new Point(point.X - size.Width / 2, point.Y - size.Height / 2);
                rectangle = new Rectangle(location, size);
            } while (rectangle.IntersectsWith(rectangles));

            return rectangle;
        }
    }
}