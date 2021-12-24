using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Templates;

namespace TagCloudTests;

[TestFixture]
public class VisualizerTests
{
    private Visualizer sut;

    [SetUp]
    public void SetUp()
    {
        sut = new Visualizer();
    }

    [Test]
    public void Draw_ShouldReturnBitmap()
    {
        var template = new Template
        {
            ImageSize = new Size(1, 1)
        };

        var result = sut.Draw(template);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new Bitmap(1, 1));
    }
}