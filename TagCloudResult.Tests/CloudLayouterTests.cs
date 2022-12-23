using System.Drawing;
using ResultOfTask;
using TagCloudResult.Curves;

namespace TagCloudResult.Tests;

[TestFixture]
public class CloudLayouterTests
{
    [SetUp]
    public void SetUp()
    {
        _curve = new ArchimedeanSpiral();
        _layouter = new CloudLayouter();
    }

    private CloudLayouter _layouter;
    private ICurve _curve;

    private IEnumerable<Rectangle> PutAllRectangles(IReadOnlyList<Size> rectangles)
    {
        return rectangles.Select(rectangleSize => _layouter.PutRectangle(_curve, rectangleSize).GetValueOrThrow()).ToList();
    }

    private static IEnumerable<TestCaseData> SizesOfRectangles
    {
        get
        {
            yield return new TestCaseData(
                new List<Size>
                {
                    new(100, 100),
                    new(100, 100),
                    new(100, 100),
                    new(100, 100),
                    new(100, 100)
                });
            yield return new TestCaseData(
                new List<Size>
                {
                    new(100, 100),
                    new(200, 100),
                    new(100, 200),
                    new(1000, 1000),
                    new(100, 100)
                });
        }
    }

    [TestCaseSource(nameof(SizesOfRectangles))]
    public void PutNextRectangle_ShouldNotMakeIntersections(IReadOnlyList<Size> sizesOfRectangles)
    {
        var rectangles = PutAllRectangles(sizesOfRectangles);

        foreach (var rectangle1 in rectangles)
        {
            foreach (var rectangle2 in rectangles)
            {
                if (rectangle1 == rectangle2)
                    continue;
                rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
            }
        }
    }
}