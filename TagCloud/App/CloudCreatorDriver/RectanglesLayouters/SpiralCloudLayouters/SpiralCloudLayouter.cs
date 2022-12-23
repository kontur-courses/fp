using System.Drawing;

namespace TagCloud.App.CloudCreatorDriver.RectanglesLayouters.SpiralCloudLayouters;

public class SpiralCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> laidRectangles = new();
    private double rotationAngle;
    private SpiralCloudLayouterSettings? settings;

    public Result<List<Rectangle>> GetLaidRectangles(IEnumerable<Size> sizes, ICloudLayouterSettings layouterSettings)
    {
        return SetSettings(layouterSettings)
            .Then(_ =>
            {
                foreach (var size in sizes)
                {
                    var result = PutNextRectangle(size);
                    if (!result.IsSuccess)
                        return Result.Fail<None>("Rectangles didn't lay. " + result.Value);
                }

                return Result.Ok();
            })
            .Then(_ => laidRectangles);
    }

    private Result<None> SetSettings(ICloudLayouterSettings layouterSettings)
    {
        if (layouterSettings is not SpiralCloudLayouterSettings spiralLayouterSettings)
            return Result.Fail<None>("Incorrect layouter settings type. " +
                                     $"Expected {typeof(SpiralCloudLayouterSettings)}, " +
                                     $"found {layouterSettings.GetType()}");
        settings = spiralLayouterSettings;
        return Result.Ok();
    }

    private Result<None> PutNextRectangle(Size rectangleSize)
    {
        if (settings == null)
            return Result.Fail<None>("Settings are not initialised");

        Result<Rectangle> rectangleCreation;
        if (laidRectangles.Count == 0)
            rectangleCreation = new Rectangle(
                settings.Center.X - rectangleSize.Width / 2,
                settings.Center.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height).AsResult();
        else rectangleCreation = FindNextRectangleOnSpiral(rectangleSize);

        return rectangleCreation
            .Then(rect => laidRectangles.Add(rect));
    }

    private Result<Rectangle> FindNextRectangleOnSpiral(Size rectangleSize)
    {
        // This cycle is obviously interrupted, because you can always find the location for the next rectangle
        while (true)
        {
            var newRectangle = GetNextPositionOnSpiral()
                .Then(p => new Point(p.X + settings!.Center.X, p.Y + settings!.Center.Y))
                .Then(point => GetPositionedRectangle_DependedOnAngle(point, rectangleSize))
                .RefineError("Can not find rectangle in spiral");
            if (!newRectangle.IsSuccess ||
                laidRectangles.All(rectangle => !newRectangle.Value.IntersectsWith(rectangle))) return newRectangle;
        }
    }

    private Rectangle GetPositionedRectangle_DependedOnAngle(Point position, Size rectangleSize)
    {
        var angle = rotationAngle % (2 * Math.PI);
        var x = position.X;
        var y = position.Y;
        int left, top;
        if (Math.Abs(angle - Math.PI / 2) < 1e-4)
        {
            left = x - rectangleSize.Width / 2;
            top = y - rectangleSize.Height;
            return new Rectangle(left, top, rectangleSize.Width, rectangleSize.Height);
        }

        if (Math.Abs(angle - 3 * Math.PI / 2) < 1e-4)
        {
            left = x - rectangleSize.Width / 2;
            top = y;
            return new Rectangle(left, top, rectangleSize.Width, rectangleSize.Height);
        }

        var quarterOfPlane = (int)Math.Ceiling(2 * angle / Math.PI);
        switch (quarterOfPlane)
        {
            case 1:
                left = x;
                top = y - rectangleSize.Height;
                break;
            case 2:
                left = x - rectangleSize.Width;
                top = y - rectangleSize.Height;
                break;
            case 3:
                left = x - rectangleSize.Width;
                top = y;
                break;
            default:
                left = x;
                top = y;
                break;
        }

        return new Rectangle(left, top, rectangleSize.Width, rectangleSize.Height);
    }

    private Result<Point> GetNextPositionOnSpiral()
    {
        var radius = GetPolarRadiusByAngleOnSpiral(rotationAngle);
        if (!radius.IsSuccess)
            return Result.Fail<Point>("Can not find position of next rectangle. " + radius.Error);
        var x = radius.Value * Math.Cos(rotationAngle);
        var y = radius.Value * Math.Sin(rotationAngle);
        var intX = (int)Math.Round(x);
        var intY = (int)Math.Round(y);
        rotationAngle += settings!.RotationStep;
        return Result.Ok(new Point(intX, intY));
    }

    private Result<double> GetPolarRadiusByAngleOnSpiral(double angle)
    {
        return settings == null
            ? Result.Fail<double>("Settings are not initialised")
            : Result.Ok(settings.SpiralStep / (2 * Math.PI) * angle);
    }
}