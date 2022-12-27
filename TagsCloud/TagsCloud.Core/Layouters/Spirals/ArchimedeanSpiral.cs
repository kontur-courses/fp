using System.Drawing;
using TagsCloud.Core.Helpers;

namespace TagsCloud.Core.Layouters.Spirals;

public class ArchimedeanSpiral : ISpiral
{
	private readonly Point center;

	private ArchimedeanSpiral(Point center, double spiralStepInPixels, double angleStepInDegrees)
	{
		this.center = center;
		SpiralStepInPixels = spiralStepInPixels;
		AngleStepInRadians = angleStepInDegrees * Math.PI / 180;
		CurrentAngleInRadians = 0;
	}

	public double SpiralStepInPixels { get; }

	public double AngleStepInRadians { get; }

	public double CurrentAngleInRadians { get; private set; }

	public Point GetNextPoint()
	{
		var polarRadius = SpiralStepInPixels / (Math.PI * 2) * CurrentAngleInRadians;
		var x = polarRadius * Math.Cos(CurrentAngleInRadians);
		var y = polarRadius * Math.Sin(CurrentAngleInRadians);

		var localPosition = new Point(Convert.ToInt32(x), Convert.ToInt32(y));

		CurrentAngleInRadians += AngleStepInRadians;

		return localPosition.Plus(center);
	}

	public static Result<ArchimedeanSpiral> GetSpiral(Point center, double spiralStepInPixels,
		double angleStepInDegrees)
	{
		if (spiralStepInPixels == 0)
			return Result.Fail<ArchimedeanSpiral>("Spiral step should not be equals zero");
		if (angleStepInDegrees == 0)
			return Result.Fail<ArchimedeanSpiral>("Angle step should not be equals zero");

		return new ArchimedeanSpiral(center, spiralStepInPixels, angleStepInDegrees);
	}
}