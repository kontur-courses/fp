using TagsCloudVisualization.Common;

namespace TagsCloudVisualizationTests.SettingsTests;

public class ImageSettingsTests
{
    private const int CorrectWidth = 100;
    private const int CorrectHeight = 100;

    [Test]
    public void Validate_ResultIsSuccess_WhenArgumentsOk()
    {
        var settings = new ImageSettings
        {
            Width = CorrectWidth,
            Height = CorrectHeight
        };
        
        settings.Validate().IsSuccess.Should().BeTrue();
    }
    
    [TestCase(0, CorrectHeight, TestName = "width shouldn't equals zero")]
    [TestCase(-10, CorrectHeight, TestName = "width shouldn't less than zero")]
    [TestCase(CorrectWidth, 0, TestName = "height shouldn't equals zero")]
    [TestCase(CorrectWidth, -10, TestName = "height shouldn't less than zero")]
    public void Validate_ResultIsFail_WhenArgumentsIncorrect(int width, int height)
    {
        var settings = new ImageSettings
        {
            Width = width,
            Height = height
        };
        
        settings.Validate().IsSuccess.Should().BeFalse();
    }
}
