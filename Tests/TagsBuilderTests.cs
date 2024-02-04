using FluentAssertions;
using System.Drawing;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Parser;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;
using TagsCloudPainter.Settings.Tag;
using TagsCloudPainter.Tags;

namespace TagsCloudPainterTests;

[TestFixture]
public class TagsBuilderTests
{
    [SetUp]
    public void Setup()
    {
        var tagSettings = new TagSettings { TagFontSize = 32 };
        tagsBuilder = new TagsBuilder(tagSettings);
    }

    private TagsBuilder tagsBuilder;

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenGivenNullITagSettings()
    {
        Assert.Throws<ArgumentNullException>(() => new TagsBuilder(null));
    }

    [Test]
    public void GetTags_ShouldReturnTagsWithGivenWords()
    {
        var words = new List<string> { "tag" };

        var tags = tagsBuilder.GetTags(words).GetValueOrThrow();

        tags[0].Value.Should().Be("tag");
    }

    [Test]
    public void GetTags_ShouldReturnTagsWithDifferentValues()
    {
        var words = new List<string> { "tag", "tag" };

        var tags = tagsBuilder.GetTags(words).GetValueOrThrow();

        tags.Count.Should().Be(1);
    }

    [Test]
    public void GetTags_ShouldReturnTagsWithCorrectCount()
    {
        var words = new List<string> { "tag", "tag" };

        var tags = tagsBuilder.GetTags(words).GetValueOrThrow();

        tags[0].Count.Should().Be(2);
    }

    private static IEnumerable<TestCaseData> FailingWords => new[]
    {
        new TestCaseData(null).SetName("WhenGivenNullWords"),
        new TestCaseData(new List<string> { string.Empty }).SetName("WhenGivenWordsWithEmptyWord"),
        new TestCaseData(new List<string> { null }).SetName("WhenGivenWordsWithNullWord"),
    };

    [TestCaseSource(nameof(FailingWords))]
    public void GetTags_ShouldFail_WhenGivenNullWords(List<string> words)
    {
        var tags = tagsBuilder.GetTags(words);

        Assert.That(tags.IsSuccess, Is.False);
    }
}