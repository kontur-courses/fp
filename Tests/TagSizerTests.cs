using System.Drawing;
using FakeItEasy;
using TagsCloudPainter.Sizer;
using TagsCloudPainter.Tags;

namespace TagsCloudPainterTests;

[TestFixture]
public class TagSizerTests
{
    [SetUp]
    public void SetUp()
    {
        stringSizer = A.Fake<IStringSizer>();
        tagSizer = new TagSizer(stringSizer);
    }

    private IStringSizer stringSizer;
    private ITagSizer tagSizer;

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenGivenNullIStringSizer()
    {
        Assert.Throws<ArgumentNullException>(() => new TagSizer(null));
    }

    private static IEnumerable<TestCaseData> FailingSizes => new[]
    {
        new TestCaseData(new Size(0, 10)).SetName("WidthNotPossitive"),
        new TestCaseData(new Size(10, 0)).SetName("HeightNotPossitive"),
        new TestCaseData(new Size(0, 0)).SetName("HeightAndWidthNotPossitive")
    };

    [TestCaseSource(nameof(FailingSizes))]
    public void GetTagSize_ShouldFail_(Size size)
    {
        A.CallTo(() => stringSizer.GetStringSize(A<string>.Ignored, A<FontFamily>.Ignored, A<float>.Ignored))
            .Returns(size);

        var result = tagSizer.GetTagSize(new Tag("a", 2, 1), FontFamily.Families.First());

        Assert.That(result.IsSuccess, Is.False);
    }
}