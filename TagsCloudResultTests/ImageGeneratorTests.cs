using System.Drawing;
using FluentAssertions;
using TagsCloudResult.Image;
using TagsCloudResult.UI;

namespace TagsCloudResultTests;

[TestFixture]
public class ImageGeneratorTests
{
    private ImageGenerator imageGenerator = null!;

    [OneTimeSetUp]
    public void SetUp()
    {
        var args = new ApplicationArguments { FontPath = "wrong" };
        imageGenerator = new ImageGenerator(args);
    }

    [Test]
    public void GetOuterRectangle_Should_ReturnFail_IfFontNotFound()
    {
        imageGenerator.GetOuterRectangle("word", 1).IsErr.Should().BeTrue();
    }

    [Test]
    public void DrawTagCloud_Should_ReturnFail_IfFontNotFound()
    {
        imageGenerator.DrawTagCloud([("word", 1, new Rectangle(1, 1, 1, 1))])
            .IsErr.Should().BeTrue();
    }
}