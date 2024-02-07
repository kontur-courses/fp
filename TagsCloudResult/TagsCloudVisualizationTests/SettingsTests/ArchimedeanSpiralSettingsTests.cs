using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.PointsProviders;

namespace TagsCloudVisualizationTests.SettingsTests;

public class ArchimedeanSpiralSettingsTests
{
    private const double CorrectDeltaAngle = 0.5;
    private const double CorrectDistance = 1;

    [Test]
    public void Validate_ResultIsSuccess_WhenArgumentsOk()
    {
        var settings = new ArchimedeanSpiralSettings
        {
            Center = Point.Empty,
            DeltaAngle = CorrectDeltaAngle,
            Distance = CorrectDistance
        };
        
        settings.Validate().IsSuccess.Should().BeTrue();
    }
    
    [TestCase(0, CorrectDistance, TestName = "deltaAngle shouldn't equals zero")]
    [TestCase(CorrectDeltaAngle, 0, TestName = "distance shouldn't equals zero")]
    public void Validate_ResultIsFail_WhenArgumentsIncorrect(double deltaAngle, double distance)
    {
        var settings = new ArchimedeanSpiralSettings
        {
            Center = Point.Empty,
            DeltaAngle = deltaAngle,
            Distance = distance
        };
        
        settings.Validate().IsSuccess.Should().BeFalse();
    }
}
