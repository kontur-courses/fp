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