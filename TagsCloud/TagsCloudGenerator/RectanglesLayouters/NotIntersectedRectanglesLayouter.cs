using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FailuresProcessing;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.RectanglesLayouters
{
    public class NotIntersectedRectanglesLayouter : IRectanglesLayouter
    {
        private readonly List<RectangleF> rectangles;
        private readonly IFactory<IPointsSearcher> searchersFactory;

        public NotIntersectedRectanglesLayouter(IFactory<IPointsSearcher> searchersFactory)
        {
            this.searchersFactory = searchersFactory;
            rectangles = new List<RectangleF>();
        }

        public Result<RectangleF> PutNextRectangle(SizeF rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentOutOfRangeException();

            return
                searchersFactory.CreateSingle()
                .Then(searcher =>
                {
                    var rectangle = new RectangleF { Size = rectangleSize };
                    rectangle.Location = FindFreeLocation(rectangle, searcher);
                    rectangles.Add(rectangle);
                    return Result.Ok(rectangle);
                })
                .RefineError($"{typeof(NotIntersectedRectanglesLayouter)} failure");
        }

        public Result<None> Reset()
        {
            return 
                searchersFactory.CreateSingle()
                .Then(searcher =>
                {
                    rectangles.Clear();
                    return searcher.Reset();
                });
        }

        private PointF FindFreeLocation(RectangleF rectangle, IPointsSearcher searcher)
        {
            do rectangle = rectangle.SetCenter(searcher.GetNextPoint());
            while (rectangles.Any(r => r.IntersectsWith(rectangle)));
            return rectangle.Location;
        }
    }

    internal static class RectangleExtensions
    {
        public static RectangleF SetCenter(this RectangleF rectangle, PointF center)
        {
            rectangle.Location = new PointF(center.X - rectangle.Width / 2, center.Y - rectangle.Height / 2);
            return rectangle;
        }
    }
}