using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagCloud
{
    public class CloudLayouter : ICloudLayouter
    {
        private readonly IEnumerable<IPlacementStrategy> strategies;
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CloudLayouter(IEnumerable<IPlacementStrategy> strategies)
        {
            this.strategies = strategies;
            Center = new Point();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>("Rectangle had a non-positive dimension");
            var rectangle = new Rectangle(Center, rectangleSize);
            rectangle = strategies.Aggregate(rectangle,
                (current, strategy) => strategy.PlaceRectangle(current, Rectangles.ToArray()));
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
