﻿using System.Drawing;
using TagsCloudPainter;
using TagsCloudPainter.Drawer;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.Tag;
using TagsCloudPainter.Tags;

namespace TagsCloudPainterTests;

[TestFixture]
public class CloudDrawerTests
{
    [SetUp]
    public void Setup()
    {
        var cloudSettings = new CloudSettings { CloudCenter = new Point(0, 0), BackgroundColor = Color.White };
        var tagSettings = new TagSettings { TagFontSize = 32, TagColor = Color.Black };
        drawer = new CloudDrawer(tagSettings, cloudSettings);
    }

    private ICloudDrawer drawer;

    private static IEnumerable<TestCaseData> DrawArgumentException => new[]
    {
        new TestCaseData(new TagsCloud(new Point(0, 0),
                new List<(Tag, Rectangle)> { (new Tag("a", 1, 1), new Rectangle(1, 1, 1, 1)) }), 0, 1)
            .SetName("WhenGivenNotPositiveImageWidth"),
        new TestCaseData(new TagsCloud(new Point(0, 0),
                new List<(Tag, Rectangle)> { (new Tag("a", 1, 1), new Rectangle(1, 1, 1, 1)) }), 1, 0)
            .SetName("WhenGivenNotPositiveImageHeight"),
        new TestCaseData(new TagsCloud(new Point(0, 0),
                new List<(Tag, Rectangle)> { (new Tag("a", 1, 1), new Rectangle(1, 1, 1, 1)) }), 0, 0)
            .SetName("WhenGivenNotPositiveImageHeightAndWidth"),
        new TestCaseData(new TagsCloud(new Point(0, 0), new List<(Tag, Rectangle)>()), 1, 1)
            .SetName("WhenGivenCloudWithEmptyTagsDictionary")
    };

    [TestCaseSource(nameof(DrawArgumentException))]
    public void Draw_ShouldFail(TagsCloud cloud, int width, int height)
    {
        var result = drawer.DrawCloud(cloud, width, height);

        Assert.That(result.IsSuccess, Is.False);
    }

    private static IEnumerable<TestCaseData> DrawNoException => new[]
    {
        new TestCaseData(new TagsCloud(new Point(5, 5),
                new List<(Tag, Rectangle)> { (new Tag("abcdadg", 10, 1), new Rectangle(5, 5, 20, 3)) }), 10, 10)
            .SetName("WhenCloudWidthIsGreaterThanImageWidth"),
        new TestCaseData(new TagsCloud(new Point(5, 5),
                new List<(Tag, Rectangle)> { (new Tag("abcdadg", 10, 1), new Rectangle(5, 5, 3, 20)) }), 10, 10)
            .SetName("WhenCloudHeightIsGreaterThanImageHeight"),
        new TestCaseData(new TagsCloud(new Point(5, 5),
                new List<(Tag, Rectangle)> { (new Tag("abcdadg", 10, 1), new Rectangle(5, 5, 20, 20)) }), 10, 10)
            .SetName("WhenCloudIsBiggerThanImage")
    };

    [TestCaseSource(nameof(DrawNoException))]
    public void Draw_ShouldNotFail(TagsCloud cloud, int width, int height)
    {
        var result = drawer.DrawCloud(cloud, width, height);

        Assert.That(result.IsSuccess, Is.True);
    }
}