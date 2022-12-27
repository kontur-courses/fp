using System.Collections.Immutable;
using System.Drawing;
using FluentResults;
using QuadTrees;
using QuadTrees.Wrappers;
using TagCloud.Abstractions;

namespace TagCloud;

public class BaseCloudLayouter : ICloudLayouter
{
    private readonly IEnumerator<Point> pointEnumerator;
    private readonly QuadTreeRect<QuadTreeRectWrapper> rectanglesQuadTree;

    public BaseCloudLayouter(Point center, IPointGenerator pointGenerator)
    {
        Center = center;
        pointEnumerator = pointGenerator.Generate(center).GetEnumerator();
        rectanglesQuadTree = new QuadTreeRect<QuadTreeRectWrapper>(
            int.MinValue / 2 + center.X, int.MinValue / 2 + center.Y,
            int.MaxValue, int.MaxValue);
    }

    public Point Center { get; }

    public ImmutableArray<Rectangle> Rectangles => rectanglesQuadTree
        .GetAllObjects()
        .Select(rw => rw.Rect).ToImmutableArray();

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (!rectangleSize.IsPositive())
            return Result.Fail($"Width and height of the rectangle must be positive, but {rectangleSize}.");

        Rectangle? rectangle = null;
        while (rectangle is null)
        {
            var createResult = CreateNewRectangle(rectangleSize);
            if (createResult.IsFailed) return createResult;
            var possibleRect = createResult.Value;
            if (!rectanglesQuadTree.GetObjects(possibleRect).Any())
                rectangle = possibleRect;
        }

        rectanglesQuadTree.Add(new QuadTreeRectWrapper(rectangle.Value));

        return rectangle;
    }

    private Result<Rectangle> CreateNewRectangle(Size size)
    {
        if (!pointEnumerator.MoveNext())
            return Result.Fail("You are trying to put a new rectangle, but the points sequence has ended.");

        var point = new Point(pointEnumerator.Current.X - size.Width / 2,
            pointEnumerator.Current.Y - size.Height / 2);
        return new Rectangle(point, size);
    }
}