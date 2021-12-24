﻿using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.PointGenerator;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly IPointGenerator pointGenerator;
        private readonly List<Rectangle> rectangles = new();

        public CircularCloudLayouter(IPointGenerator pointGenerator) => this.pointGenerator = pointGenerator;

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            return rectangleSize.AsResult()
                .Validate(x => x.Width > 0 && x.Height > 0,
                    $"rectangle's width and height must be positive, but was: {rectangleSize}")
                .Then(GetFirstCorrectRectangle)
                .Then(ShiftRectangleToCenter)
                .Then(x =>
                {
                    rectangles.Add(x);
                    return x;
                });
        }

        private Rectangle GetFirstCorrectRectangle(Size rectangleSize)
        {
            var rectangleCenter = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);
            while (true)
            {
                var point = pointGenerator.GenerateNextPoint();
                var rectangle = new Rectangle(point - rectangleCenter, rectangleSize);
                if (!rectangle.IntersectsWith(rectangles))
                    return rectangle;
            }
        }

        private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
        {
            var rectangleCenter = rectangle.GetCenter();
            var direction = new Point(pointGenerator.Center.X - rectangleCenter.X,
                pointGenerator.Center.Y - rectangleCenter.Y);
            var offset = new Point(Math.Sign(direction.X), Math.Sign(direction.Y));

            return Shift(Shift(rectangle, new Point(offset.X, 0)), new Point(0, offset.Y));
        }

        private Rectangle Shift(Rectangle rectangle, Point offset)
        {
            var shiftingRectangle = rectangle;
            while (!shiftingRectangle.IntersectsWith(rectangles)
                   && !shiftingRectangle.GetCenter().IsOnTheSameAxisWith(pointGenerator.Center))
            {
                rectangle = shiftingRectangle;
                shiftingRectangle.Offset(offset);
            }

            return rectangle;
        }
    }
}