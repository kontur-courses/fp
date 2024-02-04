using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using TagsCloudPainter.CloudLayouter;
using TagsCloudPainter.Drawer;
using TagsCloudPainter.Extensions;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;
using TagsCloudPainter.Settings.Tag;
using TagsCloudPainter.Sizer;
using TagsCloudPainter.Tags;

namespace TagsCloudPainterTests;

[TestFixture]
public class TagsCloudLayouterTests
{
    [SetUp]
    public void Setup()
    {
        var cloudSettings = new CloudSettings { CloudCenter = new Point(0, 0) };
        tagSettings = new TagSettings { TagFontSize = 32 };
        var pointerSettings = new SpiralPointerSettings { AngleConst = 1, RadiusConst = 0.5, Step = 0.1 };
        var formPointer = new ArchimedeanSpiralPointer(cloudSettings, pointerSettings);
        stringSizer = A.Fake<IStringSizer>();
        A.CallTo(() => stringSizer.GetStringSize(A<string>.Ignored, A<FontFamily>.Ignored, A<float>.Ignored))
            .Returns(new Size(10, 10));
        tagsCloudLayouter = new TagsCloudLayouter(cloudSettings, formPointer, tagSettings, stringSizer);
    }

    private TagsCloudLayouter tagsCloudLayouter;
    private ITagSettings tagSettings;
    private IStringSizer stringSizer;

    private static IEnumerable<TestCaseData> ConstructorArgumentNullExceptions => new[]
    {
        new TestCaseData(null, 
            A.Fake<ArchimedeanSpiralPointer>(),
            A.Fake<TagSettings>(),
            A.Fake<StringSizer>())
        .SetName("WhenGivenNullCloudSettings"),
        new TestCaseData(A.Fake<CloudSettings>(),
            null,
            A.Fake<TagSettings>(),
            A.Fake<StringSizer>())
        .SetName("WhenGivenNullFormPointer"),
        new TestCaseData(A.Fake<CloudSettings>(),
            A.Fake<ArchimedeanSpiralPointer>(),
            null,
            A.Fake<StringSizer>())
        .SetName("WhenGivenNullTagSettings"),
        new TestCaseData(A.Fake<CloudSettings>(),
            A.Fake<ArchimedeanSpiralPointer>(),
            A.Fake<TagSettings>(),
            null)
        .SetName("WhenGivenNullStringSizer"),
    };

    [TestCaseSource(nameof(ConstructorArgumentNullExceptions))]
    public void Constructor_ShouldThrowArgumentNullException_(
        ICloudSettings cloudSettings,
        IFormPointer formPointer,
        ITagSettings tagSettings,
        IStringSizer stringSizer)
    {
        Assert.Throws<ArgumentNullException>(() => new TagsCloudLayouter(cloudSettings, formPointer, tagSettings, stringSizer));
    }

    private static IEnumerable<TestCaseData> FailingSizes => new[]
    {
        new TestCaseData(new Size(0, 10)).SetName("WidthNotPossitive"),
        new TestCaseData(new Size(10, 0)).SetName("HeightNotPossitive"),
        new TestCaseData(new Size(0, 0)).SetName("HeightAndWidthNotPossitive")
    };

    [TestCaseSource(nameof(FailingSizes))]
    public void PutNextRectangle_ShouldFail_WhenGivenTagWith(Size size)
    {
        A.CallTo(() => stringSizer.GetStringSize(A<string>.Ignored, A<FontFamily>.Ignored, A<float>.Ignored))
            .Returns(size);

        var result = tagsCloudLayouter.PutNextTag(new Tag("a", 2, 1));

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public void PutNextTag_ShouldReturnRectangleOfTheTagValueSize()
    {
        var tag = new Tag("ads", 10, 5);
        var tagSize = stringSizer.GetStringSize(tag.Value, tagSettings.TagFont, tag.FontSize).GetValueOrThrow();

        var resultRectangle = tagsCloudLayouter.PutNextTag(tag).GetValueOrThrow();

        resultRectangle.Size.Should().Be(tagSize);
    }

    [Test]
    public void PutNextTag_ShouldReturnRectangleThatDoesNotIntersectWithAlreadyPutOnes()
    {
        var firstTag = new Tag("ads", 10, 5);
        var secondTag = new Tag("ads", 10, 5);
        var firstPutRectangle = tagsCloudLayouter.PutNextTag(firstTag).GetValueOrThrow();
        var secondPutRectangle = tagsCloudLayouter.PutNextTag(secondTag).GetValueOrThrow();

        var doesRectanglesIntersect = firstPutRectangle.IntersectsWith(secondPutRectangle);

        doesRectanglesIntersect.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ShouldPutRectangleWithCenterInTheCloudCenter()
    {
        var center = tagsCloudLayouter.GetCloud().Center;
        var tag = new Tag("ads", 10, 5);
        var firstRectangle = tagsCloudLayouter.PutNextTag(tag).GetValueOrThrow();
        var firstRectangleCenter = firstRectangle.GetCenter();

        firstRectangleCenter.Should().Be(center);
    }

    [Test]
    public void PutTags_Fails_WhenGivenEmptyDictionary()
    {
        var result = tagsCloudLayouter.PutTags([]);

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public void GetCloud_ReturnsAsManyTagsAsWasPut()
    {
        tagsCloudLayouter.PutNextTag(new Tag("ads", 10, 5));
        tagsCloudLayouter.PutNextTag(new Tag("ads", 10, 5));
        var rectanglesAmount = tagsCloudLayouter.GetCloud().Tags.Count;

        rectanglesAmount.Should().Be(2);
    }

    private static IEnumerable<TestCaseData> FailingTags => new[]
    {
        new TestCaseData(new List<Tag>()).SetName("WhenGivenEmptyTags"),
        new TestCaseData(null).SetName("WhenGivenNullTags"),
    };

    [TestCaseSource(nameof(FailingTags))]
    public void PutTags_ShouldFail_(List<Tag> tags)
    {
        var result = tagsCloudLayouter.PutTags(tags);

        Assert.That(result.IsSuccess, Is.False);
    }
}