using System.Drawing;
using TagsCloudVisualization.Common;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsProviders;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter : BaseCloudLayouter<ArchimedeanSpiralPointsProvider>
{
    public CircularCloudLayouter(ArchimedeanSpiralPointsProvider pointsProvider) : base(pointsProvider) {}

    protected override Result<Point> FindPositionForRectangle(Size rectangleSize)
    {
        var offsetToCenter = new Size(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
        return PointsProvider.GetPoints()
            .Then(points => points
                .Select(x => x.WithOffset(offsetToCenter))
                .First(x => IsPlaceSuitableForRectangle(new Rectangle(x, rectangleSize))))
            .Then(point => MovePointToCenter(point, rectangleSize));
    }
    
    private Result<Point> MovePointToCenter(Point point, Size rectangleSize)
    {
        point = MovePointAlongXAxis(point, rectangleSize);
        point = MovePointAlongYAxis(point, rectangleSize); 
        
        return point;
    }

    private Point MovePointAlongXAxis(Point point, Size rectangleSize)
    {
        var offsetToCenter = new Size(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
        var offsetX = Math.Sign(Center.X + offsetToCenter.Width - point.X);
        while (Center.X + offsetToCenter.Width - point.X != 0 && offsetX != 0)
        {
            var newPoint = point.WithOffset(new Size(offsetX, 0));
            if (!IsPlaceSuitableForRectangle(new Rectangle(newPoint, rectangleSize)))
                break;

            point = newPoint;
        }

        return point;
    }

    private Point MovePointAlongYAxis(Point point, Size rectangleSize)
    {
        var offsetToCenter = new Size(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
        var offsetY = Math.Sign(Center.Y + offsetToCenter.Height - point.Y);
        while (Center.Y + offsetToCenter.Height - point.Y != 0 && offsetY != 0)
        {
            var newPoint = point.WithOffset(new Size(0, offsetY));
            if (!IsPlaceSuitableForRectangle(new Rectangle(newPoint, rectangleSize)))
                break;
            
            point = newPoint;
        }

        return point;
    }
}
