using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Layout
{
    public class CircularCloudLayout : IRectangleLayout
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles;
        public IEnumerable<Rectangle> Rectangles => rectangles.ToImmutableList();

        public Point Center => spiral.Center;
        public double Area => Math.Pow(spiral.Radius, 2) * Math.PI;

        public readonly ImageSettings Settings;

        public CircularCloudLayout(ImageSettings settings)
        {
            Settings = settings;
            var center = new Point(settings.Size.Width / 2, settings.Size.Height / 2);
            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height > Settings.Size.Height || rectangleSize.Width > Settings.Size.Width)
                return Result.Fail<Rectangle>("Rectangle doesn't fit in");

            Rectangle rectangle;
            do
            {
                var location = spiral.GetNextLocation();
                rectangle = new Rectangle((int) location.X, (int) location.Y, rectangleSize.Width,
                    rectangleSize.Height);

            } while (rectangles.Any(rectangle.IntersectsWith));

            if (!IsInside(rectangle))
                return Result.Fail<Rectangle>("Rectangle doesn't fit in");

            rectangles.Add(rectangle);
            return Result.Ok(rectangle);
        }

        private bool IsInside(Rectangle rectangle)
        {
            var imageRectangle = new Rectangle(new Point(0, 0), Settings.Size);
            return rectangle.Equals(Rectangle.Intersect(rectangle, imageRectangle));
        }
    }
}