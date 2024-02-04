using System.Drawing;
using TagsCloudVisualization.PointCreators;
using Results;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectanglesInLayout;
    private readonly IEnumerator<Result<Point>> pointsOnSpiral;

    public CircularCloudLayouter(IPointCreator pointCreator)
    {
        rectanglesInLayout = new();
        pointsOnSpiral = pointCreator.GetPoints().GetEnumerator();
    }

    public IList<Rectangle> Rectangles { get => rectanglesInLayout; }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            return Result.Fail<Rectangle>("Rectangle width and height must be positive");
        }

        var currentRectangle = CreateNewRectangle(rectangleSize);
        if (!currentRectangle.IsSuccess)
            return Result.Fail<Rectangle>(currentRectangle.Error);
        rectanglesInLayout.Add(currentRectangle.Value);

        return Result.Ok(currentRectangle.Value);
    }

    private Result<Rectangle> CreateNewRectangle(Size rectangleSize)
    {
        Result<Rectangle> rectangle;

        do
        {
            pointsOnSpiral.MoveNext();
            if (!pointsOnSpiral.Current.IsSuccess)
                return Result.Fail<Rectangle>(pointsOnSpiral.Current.Error);
            var rectangleLocation = GetLeftUpperCornerFromRectangleCenter(pointsOnSpiral.Current.Value, rectangleSize);
            rectangle = Result.Ok(new Rectangle(rectangleLocation, rectangleSize));
        }
        while (rectanglesInLayout.Any(rect => rect.IntersectsWith(rectangle.Value)));

        return rectangle;
    }

    private Point GetLeftUpperCornerFromRectangleCenter(Point rectangleCenter, Size rectangleSize)
    {
        return rectangleCenter - rectangleSize / 2;
    }
}