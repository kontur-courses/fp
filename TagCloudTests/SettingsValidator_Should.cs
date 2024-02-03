using TagCloud.AppSettings;

namespace TagCloudTests;

[TestFixture]
public class SettingsValidator_Should
{
    private Settings settings;

    [SetUp]
    public void SetUp()
    {
        settings = new Settings();
    }

    [TestCase(-1, 100, TestName = "Width is negative")]
    [TestCase(100, -1, TestName = "Height is negative")]
    [TestCase(-1, -1, TestName = "Both width and height is negative")]
    [TestCase(0, 100, TestName = "Width is zero")]
    [TestCase(100, 0, TestName = "Height is zero")]
    [TestCase(0, 0, TestName = "Both width and height are zero")]
    public void ReturnFail_OnWrongSizeParameters(int width, int height)
    {
        settings.CloudWidth = width;
        settings.CloudHeight = height;

        var result = SettingsValidator.SizeIsValid(settings);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Image size params should be positive");
    }

    [Test]
    public void ReturnSuccess_OnCorrectSizeParameters()
    {
        settings.CloudHeight = 100;
        settings.CloudWidth = 100;

        var result = SettingsValidator.SizeIsValid(settings);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void ReturnFail_WhenFontIsNotInstalled()
    {
        settings.FontType = "\n";

        var result = SettingsValidator.FontIsInstalled(settings);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"Font {settings.FontType} doesn't installed");
    }

    [Test]
    public void ReturnSuccess_OnCorrectFontFamily()
    {
        settings.FontType = "Arial";

        var result = SettingsValidator.FontIsInstalled(settings);

        result.IsSuccess.Should().BeTrue();
    }
}