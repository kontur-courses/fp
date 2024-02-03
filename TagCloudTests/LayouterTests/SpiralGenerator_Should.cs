using TagCloud.AppSettings;
using TagCloud.PointGenerator;

[TestFixture]
public class SpiralGenerator_Should
{
    private const int Width = 1920;
    private const int Height = 1080;
    private const int Density = 1;
    private SpiralGenerator sut;
    private Settings settings;

    [SetUp]
    public void Setup()
    {
        settings = new Settings { CloudWidth = Width, CloudHeight = Height, CloudDensity = Density };
        sut = new SpiralGenerator(settings);
    }

    [Test]
    public void ReturnCenterPoint_OnFirstCall()
    {
        var expected = new Point(settings.CloudWidth / 2, settings.CloudHeight / 2);

        sut.GetNextPoint().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ReturnDifferentPoints_AfterMultipleCalls()
    {
        var spiralPoints = new HashSet<Point>();

        for (var i = 0; i < 50; i++)
        {
            spiralPoints.Add(sut.GetNextPoint());
        }

        spiralPoints.Count.Should().BeGreaterThan(1);
    }
}