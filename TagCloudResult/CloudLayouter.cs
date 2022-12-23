using System.Drawing;
using ResultOfTask;
using TagCloudResult.Curves;
using TagCloudResult.Extensions;

namespace TagCloudResult;

public class CloudLayouter
{
    private readonly double _curveStep;
    private readonly List<Rectangle> _rectangles = new();
    private double _lastCurveParameter;

    public CloudLayouter(Point center, double curveStep)
    {
        Center = center;
        _curveStep = curveStep;
    }

    public CloudLayouter(double curveStep = 0.01) : this(Point.Empty, curveStep)
    {
    }

    public IEnumerable<Rectangle> Rectangles => _rectangles;
    public Point Center { get; }

    public Result<Rectangle> PutRectangle(ICurve curve, Size rectangleSize)
    {
        return Result.Of(() => new Rectangle(Point.Empty, rectangleSize))
            .Then(rectangle => PlaceRectangle(curve, rectangle))
            .Then(ShiftRectangleToCenter)
            .Then(AddRectangleToRectangles);
    }

    private Rectangle PlaceRectangle(ICurve curve, Rectangle rectangle)
    {
        do
        {
            rectangle.Location = curve.GetPoint(_lastCurveParameter) + (Size)Center;
            _lastCurveParameter += _curveStep;
        } while (rectangle.IntersectsWith(_rectangles));

        return rectangle;
    }

    private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
    {
        var dx = rectangle.GetCenter().X < Center.X ? 1 : -1;
        rectangle = ShiftRectangle(rectangle, dx, 0);
        var dy = rectangle.GetCenter().Y < Center.Y ? 1 : -1;
        rectangle = ShiftRectangle(rectangle, 0, dy);
        return rectangle;
    }

    private Rectangle ShiftRectangle(Rectangle rectangle, int dx, int dy)
    {
        var offset = new Size(dx, dy);
        while (rectangle.IntersectsWith(_rectangles) == false &&
               rectangle.GetCenter().X != Center.X &&
               rectangle.GetCenter().Y != Center.Y)
            rectangle.Location += offset;

        if (rectangle.IntersectsWith(_rectangles))
            rectangle.Location -= offset;

        return rectangle;
    }

    private Rectangle AddRectangleToRectangles(Rectangle rectangle)
    {
        _rectangles.Add(rectangle);
        return rectangle;
    }
}