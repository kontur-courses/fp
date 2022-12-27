using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Core.Layouters.Spirals;

namespace TagsCloud.Tests;

[TestFixture]
public class ArchimedeanSpiralTests
{
	[SetUp]
	public void SetUp()
	{
		spiral = ArchimedeanSpiral.GetSpiral(new Point(0, 0), 1, 1);
	}

	private ArchimedeanSpiral spiral;

	[TestCaseSource(nameof(CentersSource))]
	public void Constructor_WithCorrectCenter_ShouldReturnOkResult(Point center)
	{
		var spiralResult = ArchimedeanSpiral.GetSpiral(center, 1, 1);

		spiralResult.IsSuccess.Should().BeTrue();
	}

	[TestCase(-30, 30, TestName = "Negative spiral step")]
	[TestCase(30, -30, TestName = "Negative angle step")]
	[TestCase(30, 370, TestName = "Angle step greater than 360 degrees")]
	public void Constructor_WithCorrectParameters_ShouldReturnOkResult(double spiralStep, double angleStep)
	{
		var spiralResult = ArchimedeanSpiral.GetSpiral(Point.Empty, spiralStep, angleStep);

		spiralResult.IsSuccess.Should().BeTrue();
	}

	[TestCase(0, 1, TestName = "Zero spiral step")]
	[TestCase(1, 0, TestName = "Zero angle step")]
	public void Constructor_WithIncorrectParameters_ShouldReturnFailResult(double spiralStep, double angleStep)
	{
		var spiralResult = ArchimedeanSpiral.GetSpiral(Point.Empty, spiralStep, angleStep);

		spiralResult.IsFail.Should().BeTrue();
	}

	[TestCase(123, TestName = "Positive step")]
	[TestCase(-123, TestName = "Negative step")]
	public void GetNextPoint_LengthBetweenPointsOnSameLine_ShouldBeEqualsSpiralStep(int spiralStep)
	{
		spiral = ArchimedeanSpiral.GetSpiral(new Point(0, 0), spiralStep, 360);

		var p1 = spiral.GetNextPoint();
		var p2 = spiral.GetNextPoint();
		var p3 = spiral.GetNextPoint();

		var distance1 = GetDistanceBetweenPoints(p1, p2);
		var distance2 = GetDistanceBetweenPoints(p2, p3);

		distance1.Should().Be(distance2);
	}

	private static double GetDistanceBetweenPoints(Point p1, Point p2)
	{
		return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
	}

	public static IEnumerable<TestCaseData> CentersSource()
	{
		yield return new TestCaseData(Point.Empty).SetName("Center at origin");
		yield return new TestCaseData(new Point(1, 1)).SetName("Center in the first quadrant");
		yield return new TestCaseData(new Point(-1, 1)).SetName("Center in the second quadrant");
		yield return new TestCaseData(new Point(-1, -1)).SetName("Center in the third quadrant");
		yield return new TestCaseData(new Point(1, -1)).SetName("Center in the fourth coordinate quadrant");
	}
}