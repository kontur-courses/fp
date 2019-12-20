using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.TagCloudVisualisation.Spirals;
using ResultLogic;

namespace TagCloud.TagCloudVisualisation.TagCloudLayouter
{
    public class CircularCloudLayouter : ITagCloudLayouter
    {
        public readonly Point Center;
        private readonly List<Rectangle> rectangles;
        private readonly ISpiral spiral;
        private readonly IEnumerator<Point> spiralPoints;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            spiral = new ArchimedeanSpiral();
            spiralPoints = spiral.GetPoints().GetEnumerator();
        }

        public CircularCloudLayouter(Point center, ISpiral spiral)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            this.spiral = spiral;
            spiralPoints = spiral.GetPoints().GetEnumerator();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>(new ArgumentException(
                    $"Unable to create a rectangle with width = {rectangleSize.Width} and height = {rectangleSize.Height}"));

            while(true)
            {
                var currentSpiralPoint = spiralPoints.Current;
                var currentPosition = new Point(
                    Center.X + currentSpiralPoint.X, 
                    Center.Y + currentSpiralPoint.Y);

                var rectangle = new Rectangle(currentPosition, rectangleSize);

                if (TryAddRectangle(rectangle)) 
                    return Result.Ok(rectangle);
                spiralPoints.MoveNext();
            }
        }

        private bool TryAddRectangle(Rectangle rectangle)
        {
            if (rectangles.Exists(rec => rec.IntersectsWith(rectangle))) 
                return false;

            rectangles.Add(rectangle);
            return true;
        }

        internal IEnumerable<Rectangle> GetAllRectangles() => rectangles;
    }
}
