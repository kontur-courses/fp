using System.Drawing;
using TagsCloud.Core.Helpers;
using TagsCloud.Core.Layouters.Spirals;

namespace TagsCloud.Core.Layouters;

public class CircularCloudLayouter : ICloudLayouter
{
	private readonly Point center;
	private readonly List<Rectangle> placedRectangles;

	private readonly ISpiral spiral;

	private CircularCloudLayouter(Point center, ISpiral spiral)
	{
		this.center = center;
		this.spiral = spiral;
		placedRectangles = new List<Rectangle>();
	}

	public Result<Rectangle> PutNextRectangle(Size rectangleSize)
	{
		if (rectangleSize.Width <= 0)
			return Result.Fail<Rectangle>($"Rectangle width should be greater than 0, but was {rectangleSize.Width}");

		if (rectangleSize.Height <= 0)
			return Result.Fail<Rectangle>($"Rectangle height should be greater than 0, but was {rectangleSize.Height}");

		var rectangleOnSpiral = GetRectangleOnSpiral(rectangleSize);
		var shiftedRectangle = BinaryShiftToCenter(rectangleOnSpiral);

		placedRectangles.Add(shiftedRectangle);

		return shiftedRectangle;
	}

	public static Result<ICloudLayouter> GetLayouter(Point center)
	{
		return ArchimedeanSpiral.GetSpiral(center, 10, 1)
			.RefineError("Can't create circular layouter")
			.Then(spiral => new CircularCloudLayouter(center, spiral) as ICloudLayouter);
	}

	private Rectangle GetRectangleOnSpiral(Size rectangleSize)
	{
		Rectangle newRectangle;

		do
		{
			newRectangle = RectangleCreator.GetRectangle(spiral.GetNextPoint(), rectangleSize);
		} while (IntersectsWithPlacedRectangles(newRectangle));

		return newRectangle;
	}

	private Rectangle BinaryShiftToCenter(Rectangle rectangle)
	{
		if (placedRectangles.Count == 0) return rectangle;

		var newLocation = BinaryShiftToPoint(rectangle, center);
		newLocation = BinaryAxisShift(RectangleCreator.GetRectangle(newLocation, rectangle.Size));

		return RectangleCreator.GetRectangle(newLocation, rectangle.Size);
	}

	private Point BinaryShiftToPoint(Rectangle rectangle, Point destination)
	{
		var shiftedRectangle = rectangle;
		var left = 0.0;
		var (right, _) = CoordinatesConverter.ToPolar(rectangle.Center().Minus(destination));

		while (right > left)
		{
			var (polarRadius, polarAngle)
				= CoordinatesConverter.ToPolar(shiftedRectangle.Center().Minus(destination));

			var shiftStep = Math.Ceiling((right - left) / 2);
			var nextLocation = CoordinatesConverter.ToCartesian(polarRadius - shiftStep, polarAngle).Plus(destination);

			if (CanPlaceRectangleWithoutIntersectsWithPlaced(nextLocation, shiftedRectangle.Size))
			{
				right -= shiftStep;
				shiftedRectangle = RectangleCreator.GetRectangle(nextLocation, shiftedRectangle.Size);
			}
			else
			{
				left += shiftStep;
			}
		}

		return shiftedRectangle.Center();
	}

	private Point BinaryAxisShift(Rectangle rectangle)
	{
		var newRectangle = rectangle;
		var nextLocation = rectangle.Center();

		while (true)
		{
			nextLocation = BinaryShiftToPoint(
				RectangleCreator.GetRectangle(nextLocation, newRectangle.Size)
				, new Point(center.X, newRectangle.Center().Y));

			nextLocation = BinaryShiftToPoint(
				RectangleCreator.GetRectangle(nextLocation, newRectangle.Size)
				, new Point(newRectangle.Center().X, center.Y));

			if (nextLocation == newRectangle.Center()) break;

			newRectangle = RectangleCreator.GetRectangle(nextLocation, newRectangle.Size);
		}

		return newRectangle.Center();
	}

	private bool CanPlaceRectangleWithoutIntersectsWithPlaced(Point location, Size size)
	{
		return !IntersectsWithPlacedRectangles(RectangleCreator.GetRectangle(location, size));
	}

	private bool IntersectsWithPlacedRectangles(Rectangle rectangle)
	{
		return placedRectangles.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle));
	}
}