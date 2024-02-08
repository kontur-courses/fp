using System.Drawing;
using FluentAssertions;
using TagsCloud.Distributors;
using TagsCloud.Options;

namespace TagsCloudTests.Distributors;

[TestFixture]
public class SpiralDistributorTests
{
    private LayouterOptions options;
    private SpiralDistributor spiralDistributor;

    [SetUp]
    public void SetUp()
    {
        options = new LayouterOptions() { Center = new Point(1, 1) };
        spiralDistributor = new SpiralDistributor(options);
    }

    [Test]
    public void SprialDistributor_InitializeParams()
    {
        spiralDistributor.Center.Should().Be(options.Center);
        spiralDistributor.Angle.Should().Be(0);
        spiralDistributor.Radius.Should().Be(0);
        spiralDistributor.AngleStep.Should().Be(0.1);
        spiralDistributor.RadiusStep.Should().Be(0.1);
    }

    [Test]
    public void SpiralDistribution_ShouldReturnCenter_WhenFirstCallGetNextPoint()
    {
        spiralDistributor.GetNextPosition().GetValueOrThrow().Should().Be(spiralDistributor.Center);
    }

    [Test]
    public void SpiralDistribution_ShouldIncreaseAngle_WhenCallGetNextPoint()
    {
        spiralDistributor.GetNextPosition();
        spiralDistributor.Angle.Should().Be(spiralDistributor.AngleStep);
    }

    [Test]
    public void SpiralDistribution_ShouldIncreaseRadius_WhenAngleMoreThan2Pi()
    {
        var expectedAngle = spiralDistributor.Angle * 64 - 2 * Math.PI;
        for (var i = 0; i < 63; i++) spiralDistributor.GetNextPosition();
        spiralDistributor.Radius.Should().Be(spiralDistributor.Radius);
    }
}