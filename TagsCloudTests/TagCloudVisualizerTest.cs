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
        var size = new Size(10, 10);
        var sut = new TagCloudVisualizer(new TagSettings(), size);
        var words = new List<WordInfo> { new("hello", 5), new("gg", 6) };
        var cloudLayouter = new CloudLayouter(new Spiral(), new Point(-1, -1));
        var result = sut.DrawTags(words, Graphics.FromImage(new Bitmap(size.Width, size.Height)),
            cloudLayouter);
        var expectedImageSize = GetImageSize(cloudLayouter.Rectangles, TagCloudVisualizer.Border);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"Облако тегов вышло за границы изображения. Поставь размер {expectedImageSize.Width}x{expectedImageSize.Height}");
    }

    private Size GetImageSize(List<Rectangle> rectangles, int border)
    {
        var leftmost = Math.Min(rectangles.Min(x => x.Left), 0);
        var rightmost = rectangles.Max(x => x.Right);
        var topmost = Math.Min(rectangles.Min(x => x.Top), 0);
        var bottommost = rectangles.Max(x => x.Bottom);
        return new Size(rightmost - leftmost + border * 2, bottommost - topmost  + border * 2);
    }
}