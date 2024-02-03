using FakeItEasy;
using TagCloud.Drawer;

namespace TagCloudTests.DependencyProvidersTest;

[TestFixture]
public class PaletteProvider_Should
{
    private IPalette palette;
    private IPaletteProvider sut;
    private const string paletteName = "test";

    [SetUp]
    public void SetUp()
    {
        palette = A.Fake<IPalette>();
        A.CallTo(() => palette.Name).Returns(paletteName);
        sut = new PaletteProvider(new List<IPalette>() { palette });
    }

    [Test]
    public void CreatePalette_WithCorrectName()
    {
        var result = sut.CreatePalette(paletteName);

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().Name.Should().Be(paletteName);
    }

    [Test]
    public void ReturnFail_OnWrongPaletteName()
    {
        var wrongPalette = paletteName + "x";

        var result = sut.CreatePalette(wrongPalette);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"Palette with name {wrongPalette} doesn't exist");
    }
}