using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using TagsCloudPainter.CloudLayouter;
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
        tagsSizer = A.Fake<ITagSizer>();
        A.CallTo(() => tagsSizer.GetTagSize(A<Tag>.Ignored, A<FontFamily>.Ignored))
            .Returns(new Size(10, 10));
        tagsCloudLayouter = new TagsCloudLayouter(cloudSettings, formPointer, tagSettings, tagsSizer);
    }

    private TagsCloudLayouter tagsCloudLayouter;
    private ITagSettings tagSettings;
    private ITagSizer tagsSizer;

    private static IEnumerable<TestCaseData> ConstructorArgumentNullExceptions => new[]
    {
        new TestCaseData(null,
                A.Fake<ArchimedeanSpiralPointer>(),
                A.Fake<TagSettings>(),
                A.Fake<TagSizer>())
            .SetName("WhenGivenNullCloudSettings"),
        new TestCaseData(A.Fake<CloudSettings>(),
                null,
                A.Fake<TagSettings>(),
                A.Fake<TagSizer>())
            .SetName("WhenGivenNullFormPointer"),
        new TestCaseData(A.Fake<CloudSettings>(),
                A.Fake<ArchimedeanSpiralPointer>(),
                null,
                A.Fake<TagSizer>())
            .SetName("WhenGivenNullTagSettings"),
        new TestCaseData(A.Fake<CloudSettings>(),
                A.Fake<ArchimedeanSpiralPointer>(),
                A.Fake<TagSettings>(),
                null)
            .SetName("WhenGivenNullStringSizer")
    };

    [TestCaseSource(nameof(ConstructorArgumentNullExceptions))]
    public void Constructor_ShouldThrowArgumentNullException_(
        ICloudSettings cloudSettings,
        IFormPointer formPointer,
        ITagSettings tagSettings,
        ITagSizer stringSizer)
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TagsCloudLayouter(cloudSettings, formPointer, tagSettings, stringSizer));
    }

    [Test]
    public void PutNextTag_ShouldReturnRectangleOfTheTagValueSize()
    {
        var tag = new Tag("ads", 10, 5);
        var tagSize = tagsSizer.GetTagSize(tag, tagSettings.TagFont).GetValueOrThrow();

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
    public void PutNextTag_ShouldPutRectangleWithCenterInTheCloudCenter()
    {
        var center = tagsCloudLayouter.GetCloud().Center;
        var tag = new Tag("ads", 10, 5);

        var firstRectangle = tagsCloudLayouter.PutNextTag(tag).GetValueOrThrow();
        var firstRectangleCenter = firstRectangle.GetCenter();

        firstRectangleCenter.Should().Be(center);
    }

    [Test]
    public void GetCloud_ReturnsAsManyTagsAsWasPut()
    {
        tagsCloudLayouter.PutNextTag(new Tag("ads", 10, 5));
        tagsCloudLayouter.PutNextTag(new Tag("ads", 10, 5));

        var rectanglesAmount = tagsCloudLayouter.GetCloud().Tags.Count;

        rectanglesAmount.Should().Be(2);
    }

    private static readonly IEnumerable<TestCaseData> PutTagsNoExcepion = new[]
    {
        new TestCaseData(null).SetName("WhenListOfTagsIsNull"),
        new TestCaseData(new List<Tag>()).SetName("WhenListOfTagsIsEmpty")
    };

    [TestCaseSource(nameof(PutTagsNoExcepion))]
    public void PutTags_ShouldNotThrow_(List<Tag> tags)
    {
        Assert.DoesNotThrow(() => tagsCloudLayouter.PutTags(tags));
    }
}