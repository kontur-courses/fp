using System.Drawing;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;

namespace TagsCloudPainterTests;

[TestFixture]
public class ArchimedeanSpiralTests
{
    private static IEnumerable<TestCaseData> GetNextPointFails => new[]
    {
        new TestCaseData(new Point(1, 1), 0, 1, 1).SetName("WhenGivenNotPositiveStep"),
        new TestCaseData(new Point(1, 1), 1, 0, 1).SetName("WhenGivenNotPositiveRadius"),
        new TestCaseData(new Point(1, 1), 1, 1, 0).SetName("WhenGivenNotPositiveAngle")
    };

    [TestCaseSource(nameof(GetNextPointFails))]
    public void GetNextPoint_ShouldFail(Point center, double step, double radius, double angle)
    {
        var cloudSettings = new CloudSettings { CloudCenter = center };
        var pointerSettings = new SpiralPointerSettings { AngleConst = angle, RadiusConst = radius, Step = step };
        var pointer = new ArchimedeanSpiralPointer(cloudSettings, pointerSettings);

        var result = pointer.GetNextPoint();
        
        Assert.That(result.IsSuccess, Is.False);
    }

    private static IEnumerable<TestCaseData> ConstructorArgumentExceptions => new[]
    {
        new TestCaseData(null, new SpiralPointerSettings()).SetName("WhenGivenNullCloudSettings"),
        new TestCaseData(new CloudSettings(), null).SetName("WhenGivenNullPointerSettings"),
        new TestCaseData(null, null).SetName("WhenGivenNullCloudSettingsAndPointerSettings"),
    };

    [TestCaseSource(nameof(ConstructorArgumentExceptions))]
    public void Constructor_ShouldThrowArgumentException_(ICloudSettings cloudSettings, ISpiralPointerSettings pointerSettings)
    {
        Assert.Throws<ArgumentNullException>(() => new ArchimedeanSpiralPointer(cloudSettings, pointerSettings));
    }
}