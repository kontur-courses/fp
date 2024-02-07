using System.Drawing;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsProviders;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter : BaseCloudLayouter<ArchimedeanSpiralPointsProvider>
{
    public CircularCloudLayouter(ArchimedeanSpiralPointsProvider pointsProvider) : base(pointsProvider) {}

    protected override Result<Point> FindPositionForRectangle(Size rectangleSize)
    {
        const int sizeDivider = 2;
        var offsetToCenter = new Size(-rectangleSize.Width / sizeDivider, -rectangleSize.Height / sizeDivider);
        
        return PointsProvider.GetPoints()
            .Then(points => points
                .Select(x => x.WithOffset(offsetToCenter))
                .First(x => IsPlaceSuitableForRectangle(new Rectangle(x, rectangleSize))))
            .Then(point => MovePointTo(point, Center.WithOffset(offsetToCenter), rectangleSize));
    }
    
    private Result<Point> MovePointTo(Point point, Point toPoint, Size rectangleSize)
    {
        var desiredOffset = new Point(toPoint.X - point.X, toPoint.Y - point.Y);
        var isSuitablePoint = new Func<Point, bool>(x => IsPlaceSuitableForRectangle(new Rectangle(x, rectangleSize)));
        
        var newPoint = Result.Of(() => MovePointAlongXAxis(point, desiredOffset).TakeWhile(isSuitablePoint).Last())
            .Then(movedPoint => MovePointAlongYAxis(movedPoint, desiredOffset).TakeWhile(isSuitablePoint).Last());

        return newPoint.IsSuccess ? newPoint : point;
    }

    private static IEnumerable<Point> MovePointAlongXAxis(Point point, Point offset)
    {
        for (var i = 0; i < Math.Abs(offset.X); i++)
        {
            yield return point;
            point.X += Math.Sign(offset.X);
        }
    }
    
    private static IEnumerable<Point> MovePointAlongYAxis(Point point, Point offset)
    {
        for (var i = 0; i < Math.Abs(offset.Y); i++)
        {
            yield return point;
            point.Y += Math.Sign(offset.Y);
        }
    }
}
