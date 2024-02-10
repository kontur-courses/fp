using System.Drawing;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.CloudLayouters;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly IImageSettings imageSettings;
    private Point center;
    private double angle;
    private double spiralStep = 1;
    private List<Rectangle> rectangles = new();
    private const double DefaultAngleStep = Math.PI / 10;
    private const double DefaultSpiralStep = 1;
    private const double FullCircle = Math.PI * 2;
    private const int SpiralStepThreshold = 10;

    public CircularCloudLayouter(IImageSettings imageSettings)
    {
        this.imageSettings = imageSettings;
        UpdateCloud();
    }

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
            return Result.Fail<Rectangle>($"Ширина и высота изображения должны быть положительными.");

        var location = GetPosition(rectangleSize);
        var rectangle = new Rectangle(location, rectangleSize);
        if (!CheckRectangleInsideImage(rectangle))
            return Result.Fail<Rectangle>($"Облако не помещается в размеры изображения.");

        rectangles.Add(rectangle);
        return rectangle;
    }

    public Result UpdateCloud()
    {
        center = new Point(imageSettings.ImageSize.Width / 2, imageSettings.ImageSize.Height / 2);
        rectangles = new List<Rectangle>();

        return Result.Ok();
    }

    private bool CheckRectangleInsideImage(Rectangle rectangle)
    {
        return rectangle is { Left: >= 0, Top: >= 0 } &&
               rectangle.Left + rectangle.Width <= imageSettings.ImageSize.Width &&
               rectangle.Top + rectangle.Height <= imageSettings.ImageSize.Height;
    }

    private Point GetPosition(Size rectangleSize)
    {
        if (rectangles.Count == 0)
        {
            center.Offset(new Point(rectangleSize / -2));
            return center;
        }

        return FindApproximatePosition(rectangleSize);
    }

    private Point FindApproximatePosition(Size rectangleSize)
    {
        var currentAngle = angle;
        while (true)
        {
            var candidateLocation = new Point(center.X + (int)(spiralStep * Math.Cos(currentAngle)),
                center.Y + (int)(spiralStep * Math.Sin(currentAngle)));
            var candidateRectangle = new Rectangle(candidateLocation, rectangleSize);

            if (!IntersectsWithAny(candidateRectangle))
            {
                rectangles.Add(candidateRectangle);
                angle = currentAngle;

                return candidateRectangle.Location;
            }

            currentAngle = CalculateAngle(currentAngle);
        }
    }

    private bool IntersectsWithAny(Rectangle candidateRectangle)
    {
        return rectangles
            .Any(candidateRectangle.IntersectsWith);
    }

    private double CalculateAngle(double currentAngle)
    {
        currentAngle += GetAngleStep();
        if (currentAngle > FullCircle)
        {
            currentAngle %= FullCircle;
            UpdateSpiral();
        }

        return currentAngle;
    }

    private void UpdateSpiral()
    {
        spiralStep += DefaultSpiralStep;
    }

    private double GetAngleStep()
    {
        var angleStep = DefaultAngleStep;
        var stepCount = (int)spiralStep / SpiralStepThreshold;
        if (stepCount > 0)
            angleStep /= stepCount;

        return angleStep;
    }
}