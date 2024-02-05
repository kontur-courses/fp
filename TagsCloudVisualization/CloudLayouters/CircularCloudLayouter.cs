using System.Drawing;
using TagsCloudVisualization.PointCreators;
using Results;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> rectanglesInLayout;
    private readonly IPointCreator pointCreator;

    public CircularCloudLayouter(IPointCreator pointCreator)
    {
        rectanglesInLayout = new();
        this.pointCreator = pointCreator;
    }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            return Result.Fail<Rectangle>("Rectangle width and height must be positive, " +
                $"but width: {rectangleSize.Width}, height: {rectangleSize.Height}");
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
            var point = pointCreator.GetNextPoint();
            var rectangleLocation = GetLeftUpperCornerFromRectangleCenter(point, rectangleSize);
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