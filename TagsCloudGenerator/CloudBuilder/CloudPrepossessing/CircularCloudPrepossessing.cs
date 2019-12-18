using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultPattern;
using TagsCloudGenerator.ShapeGenerator;

namespace TagsCloudGenerator.CloudPrepossessing
{
    public class CircularCloudPrepossessing : ITagsPrepossessing
    {
        public Point Center { get; }

        private readonly List<Rectangle> rectangles;
        private readonly IShapeGenerator shape;

        public CircularCloudPrepossessing(Point center, IShapeGenerator generator)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            shape = generator;
        }

        public CircularCloudPrepossessing(Point center) : 
            this(center, new ArchimedeanShape(center, 0.5 / (2 * Math.PI)))
        { }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>($"Size {rectangleSize} for rectangle can't be less than zero.");

            while (true)
            {
                var rectVariant = new Rectangle(shape.GetNextSpiralPoint(), rectangleSize);
                
                if (rectangles.Any(rectVariant.IntersectsWith)) continue;

                rectangles.Add(rectVariant);
                return rectVariant.AsResult();
            }
        }

        public IReadOnlyList<Rectangle> GetRectangles() => rectangles;
    }
}