using FakeItEasy;
using TagCloud.PointGenerator;

namespace TagCloudTests.DependencyProvidersTest;

[TestFixture]
public class PointGeneratorProvider_Should
{
    private IPointGenerator generator;
    private IPointGeneratorProvider sut;
    private const string generatorName = "test";

    [SetUp]
    public void SetUp()
    {
        generator = A.Fake<IPointGenerator>();
        A.CallTo(() => generator.GeneratorName).Returns(generatorName);
        sut = new PointGeneratorProvider(new List<IPointGenerator>() { generator });
    }

    [Test]
    public void CreateGenerator_WithCorrectName()
    {
        var result = sut.CreateGenerator(generatorName);

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().GeneratorName.Should().Be(generatorName);
    }

    [Test]
    public void ReturnFail_OnWrongGeneratorName()
    {
        var wrongName = generatorName + "x";

        var result = sut.CreateGenerator(wrongName);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"{wrongName} layouter is not supported");
    }
}