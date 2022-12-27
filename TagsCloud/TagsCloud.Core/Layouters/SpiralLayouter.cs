using System.Drawing;
using TagsCloud.Core.Helpers;
using TagsCloud.Core.Layouters.Spirals;

namespace TagsCloud.Core.Layouters;

public class SpiralCloudLayouter : ICloudLayouter
{
	private readonly List<Rectangle> placedRectangles;

	private readonly ISpiral spiral;

	private SpiralCloudLayouter(Point center, ISpiral spiral)
	{
		this.spiral = spiral;
		placedRectangles = new List<Rectangle>();
	}

	public Result<Rectangle> PutNextRectangle(Size rectangleSize)
	{
		var rectangleOnSpiral = GetRectangleOnSpiral(rectangleSize);

		placedRectangles.Add(rectangleOnSpiral);

		return rectangleOnSpiral;
	}

	public static Result<ICloudLayouter> GetLayouter(Point center, double spiralStepInPixels, double angleStepInDegrees)
	{
		return ArchimedeanSpiral.GetSpiral(center, spiralStepInPixels, angleStepInDegrees)
			.RefineError("Can't create spiral layouter")
			.Then(spiral => new SpiralCloudLayouter(center, spiral) as ICloudLayouter);
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

	private bool IntersectsWithPlacedRectangles(Rectangle rectangle)
	{
		return placedRectangles.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle));
	}
}