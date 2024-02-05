using System.Drawing;
using FluentAssertions;
using TagsCloud.App.Settings;
using TagsCloud.CloudLayouter;
using TagsCloud.CloudVisualizer;
using TagsCloud.WordAnalyzer;

namespace TagsCloudTests;

public class TagCloudVisualizerTest
{
    [Test]
    public void TagCloudVisualizer_WhenTagCloudOutOfBounds_ShouldBeTrowException()
    {
        var size = new Size(1, 1);
        var sut = new TagCloudVisualizer(new TagSettings(), size);
        var words = new List<WordInfo> { new("hello", 5) };
        var result = sut.DrawTags(words, Graphics.FromImage(new Bitmap(size.Width, size.Height)),
            new CloudLayouter(new Spiral(), new Point(10, 10)));
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Облако тегов вышло за границы изображения.");
    }
}