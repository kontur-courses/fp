using System.Drawing;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;

namespace TagsCloudPainterTests;

[TestFixture]
public class ArchimedeanSpiralTests
{
    private static IEnumerable<TestCaseData> ConstructorArgumentExceptions => new[]
    {
        new TestCaseData(null, new SpiralPointerSettings()).SetName("WhenGivenNullCloudSettings"),
        new TestCaseData(new CloudSettings(), null).SetName("WhenGivenNullPointerSettings"),
        new TestCaseData(null, null).SetName("WhenGivenNullCloudSettingsAndPointerSettings")
    };

    [TestCaseSource(nameof(ConstructorArgumentExceptions))]
    public void Constructor_ShouldThrowArgumentNullException_(ICloudSettings cloudSettings,
        ISpiralPointerSettings pointerSettings)
    {
        Assert.Throws<ArgumentNullException>(() => new ArchimedeanSpiralPointer(cloudSettings, pointerSettings));
    }

    private static IEnumerable<TestCaseData> GetNextPointFails => new[]
    {
        new TestCaseData(new Point(1, 1), 0, 1, 1).SetName("WhenGivenNotPositiveStep"),
        new TestCaseData(new Point(1, 1), 1, 0, 1).SetName("WhenGivenNotPositiveRadius"),
        new TestCaseData(new Point(1, 1), 1, 1, 0).SetName("WhenGivenNotPositiveAngle")
    };

    [TestCaseSource(nameof(GetNextPointFails))]
    public void GetNextPoint_ShouldFail_(Point center, double step, double radius, double angle)
    {
        var cloudSettings = new CloudSettings { CloudCenter = center };
        var pointerSettings = new SpiralPointerSettings { AngleConst = angle, RadiusConst = radius, Step = step };
        var pointer = new ArchimedeanSpiralPointer(cloudSettings, pointerSettings);

        var result = pointer.GetNextPoint();

        Assert.That(result.IsSuccess, Is.False);
    }

    private static IEnumerable<TestCaseData> GetNextPointSuccess => new[]
    {
        new TestCaseData(new Point(1, 1), 1, 1, 1).SetName("WhenGivenPositiveStepAndRadiusAndAngle"),
        new TestCaseData(new Point(1, 1), 100, 100, 100).SetName("WhenGivenPositiveStepAndRadiusAndAngleOfLargeNumbers")
    };

    [TestCaseSource(nameof(GetNextPointSuccess))]
    public void GetNextPoint_ShouldSuccess_(Point center, double step, double radius, double angle)
    {
        var cloudSettings = new CloudSettings { CloudCenter = center };
        var pointerSettings = new SpiralPointerSettings { AngleConst = angle, RadiusConst = radius, Step = step };
        var pointer = new ArchimedeanSpiralPointer(cloudSettings, pointerSettings);

        var result = pointer.GetNextPoint();

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void GetNextPoint_ShouldReturnPointThatDiffersFromPreviousPoint()
    {
        var cloudSettings = new CloudSettings { CloudCenter = new Point(1, 1) };
        var pointerSettings = new SpiralPointerSettings { AngleConst = 1, RadiusConst = 1, Step = 1 };
        var pointer = new ArchimedeanSpiralPointer(cloudSettings, pointerSettings);

        var firstPoint = pointer.GetNextPoint().GetValueOrThrow();
        var secondPoint = pointer.GetNextPoint().GetValueOrThrow();

        Assert.That(firstPoint, Is.Not.EqualTo(secondPoint));
    }
}