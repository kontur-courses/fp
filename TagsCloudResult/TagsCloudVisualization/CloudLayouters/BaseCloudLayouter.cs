using System.Drawing;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.PointsProviders;

namespace TagsCloudVisualization.CloudLayouters;

public abstract class BaseCloudLayouter<TPointsProvider> : ITagsCloudLayouter
    where TPointsProvider : IPointsProvider
{
    private readonly List<Rectangle> rectangles;
    public IEnumerable<Rectangle> Rectangles => rectangles;
    protected readonly TPointsProvider PointsProvider;
    public Point Center => PointsProvider.Start;

    protected BaseCloudLayouter(TPointsProvider pointsProvider)
    {
        ArgumentNullException.ThrowIfNull(pointsProvider);

        PointsProvider = pointsProvider;
        rectangles = new List<Rectangle>();
    }
    
    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        return ValidateRectangleSize(rectangleSize)
            .Then(FindPositionForRectangle)
            .Then(position =>
        {
            var rectangle = new Rectangle(position, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        });
    }

    private static Result<Size> ValidateRectangleSize(Size rectangleSize)
    {
        return Result.Validate(rectangleSize, size => size is {Height: > 0, Width: > 0},
            Resources.BaseCloudLayouter_ValidateRectangleSize_Fail);
    }

    protected abstract Result<Point> FindPositionForRectangle(Size rectangleSize);

    protected virtual bool IsPlaceSuitableForRectangle(Rectangle rectangle)
    {
        return rectangles.All(x => !x.IntersectsWith(rectangle));
    }

    public void Dispose()
    {
        rectangles.Clear();
    }
}
