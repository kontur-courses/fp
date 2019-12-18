using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace TagsCloud.Layouters
{
    public class CircularCloudLayouter : ITagsCloudLayouter
    {
        public Result<ImmutableList<LayoutItem>> ReallocItems(ImmutableList<LayoutItem> items)
        {
            return Result.Of(() =>
            {
                if (items.Count == 0) return default;

                var sortedItems = items.Sort((i1, i2) => i2.Rectangle.Square().CompareTo(i1.Rectangle.Square()));

                var biggestItem = sortedItems[0];
                biggestItem.Rectangle.X = -biggestItem.Rectangle.Width / 2;
                biggestItem.Rectangle.Y = -biggestItem.Rectangle.Height / 2;

                var rnd = new Random();
                foreach (var i in Enumerable.Range(1, sortedItems.Count - 1))
                {
                    var size = sortedItems[i].Rectangle.Size;
                    var minVertexDist = double.MaxValue;
                    Rectangle bestRect = default;

                    var angleOffset = rnd.NextDouble();
                    foreach (var a in Enumerable.Range(0, 36))
                    {
                        var angle = angleOffset + a * Math.PI / 18;
                        var closestRectOnRay = GetClosestPlaceWithoutIntersects(angle, size, sortedItems.Take(i));
                        var farthestVertexDist = closestRectOnRay.GetDistanceOfFathestFromCenterVertex();
                        if (farthestVertexDist < minVertexDist)
                        {
                            minVertexDist = farthestVertexDist;
                            bestRect = closestRectOnRay;
                        }
                    }

                    sortedItems[i].Rectangle = bestRect;
                }

                return sortedItems;
            }).RefineError("Layouter can't allocate tags correctly.");
        }

        private Rectangle GetClosestPlaceWithoutIntersects(double rayAngle, Size size, IEnumerable<LayoutItem> checkingItems)
        {
            var farthestIntersectionPointDistance = checkingItems
                .Max(it => it.Rectangle.GetDistanceIfIntersectsByRay(rayAngle));

            var r = Utils.LengthOfRayFromCenterOfRectangle(size, rayAngle);
            const int step = 2;
            var dist = farthestIntersectionPointDistance + r;
            Rectangle tryRect;
            do
            {
                dist += step;
                var location = new Point().FromPolar(rayAngle, dist);
                location.Offset(-size.Width / 2, -size.Height / 2);
                tryRect = new Rectangle(location, size);
            } while (checkingItems.Any(it => tryRect.IntersectsWith(it.Rectangle)));

            return tryRect;
        }
    }
}
