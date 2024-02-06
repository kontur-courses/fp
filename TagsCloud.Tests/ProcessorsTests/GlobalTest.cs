using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloud.Builders;
using TagsCloud.Entities;
using TagsCloud.Extensions;
using TagsCloud.Options;
using TagsCloud.Results;
using TagsCloudVisualization;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests.ProcessorsTests;

[TestFixture]
[TestOf(nameof(TagsCloud))]
public class GlobalTest
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var languageParts = new HashSet<string> { "S" };
        var colors = new HashSet<string> { "#FF0000", "#00FF00", "#0000FF" };
        var (width, height) = (1920, 1080);

        inputOptionsResult = new InputOptionsBuilder()
                             .AsResult()
                             .Then(builder => builder.SetWordsCase(CaseType.Upper))
                             .Then(builder => builder.SetCastPolitics(true))
                             .Then(builder => builder.SetExcludedWords(excludedWords))
                             .Then(builder => builder.SetLanguageParts(languageParts))
                             .Then(builder => builder.SetLanguagePolitics(true))
                             .Then(builder => builder.BuildOptions());

        cloudOptionsResult = new CloudOptionsBuilder()
                             .AsResult()
                             .Then(builder => builder.SetColors(colors))
                             .Then(builder => builder.SetLayout(
                                 PointGeneratorType.Spiral,
                                 new PointF((float)width / 2, (float)height / 2),
                                 0.1f,
                                 (float)Math.PI / 180))
                             .Then(builder => builder.SetColoringStrategy(ColoringStrategy.AllRandom))
                             .Then(builder => builder.SetMeasurerType(MeasurerType.Logarithmic))
                             .Then(builder => builder.SetFontFamily(Path.Join(TestDataPath, "Vollkorn-Regular.ttf")))
                             .Then(builder => builder.SetSortingType(SortType.Preserve))
                             .Then(builder => builder.SetFontBounds(minFontSize, maxFontSize))
                             .Then(builder => builder.BuildOptions());

        outputOptionsResult = new OutputOptionsBuilder()
                              .AsResult()
                              .Then(builder => builder.SetImageFormat(ImageFormat.Jpeg))
                              .Then(builder => builder.SetImageSize(width, height))
                              .Then(builder => builder.SetImageBackgroundColor("#FFFFFF"))
                              .Then(builder => builder.BuildOptions());
    }

    private HashSet<WordTagGroup> wordGroups;

    private Result<IInputProcessorOptions> inputOptionsResult;
    private Result<ICloudProcessorOptions> cloudOptionsResult;
    private Result<IOutputProcessorOptions> outputOptionsResult;

    private const int minFontSize = 45;
    private const int maxFontSize = 80;
    private const string outputPath = "tagcloud_image.jpeg";

    private readonly HashSet<string> excludedWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "Ноутбук",
        "Камера"
    };

    private readonly HashSet<string> verbs = new(StringComparer.OrdinalIgnoreCase)
    {
        "Программировать",
        "Прыгать",
        "Бегать",
        "Играть"
    };

    private readonly HashSet<string> englishWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "America",
        "Russia",
        "Germany",
        "Apple",
        "TV",
        "Join",
        "Split"
    };

    [Test]
    [Order(1)]
    public void Builders_Should_NotReturnFailResults()
    {
        TestHelper.AssertResultSuccess(inputOptionsResult);
        TestHelper.AssertResultSuccess(cloudOptionsResult);
        TestHelper.AssertResultSuccess(outputOptionsResult);
    }

    [Test]
    [Order(2)]
    public void Engine_Should_NotReturnFailsResult()
    {
        var engine = new TagCloudEngine(
            inputOptionsResult.Value,
            cloudOptionsResult.Value,
            outputOptionsResult.Value);

        var groupsResult = engine.GenerateTagCloud(Path.Join("TestData", "data.txt"), outputPath);
        TestHelper.AssertResultSuccess(groupsResult);

        wordGroups = groupsResult.Value;
    }

    [Test]
    public void Font_Should_BeInSelectedRange()
    {
        foreach (var group in wordGroups)
            group.VisualInfo.TextFont.Size.Should().BeInRange(minFontSize, maxFontSize);
    }

    [Test]
    public void WordGroups_Should_ContainOnlyRussianWords()
    {
        wordGroups.Should().NotContain(group => englishWords.Contains(group.WordInfo.Text));
    }

    [Test]
    public void WordGroups_ShouldNot_ContainVerbs()
    {
        wordGroups.Should().NotContain(group => verbs.Contains(group.WordInfo.Text));
    }

    [Test]
    public void WordGroups_ShouldNot_ContainExcludedWords()
    {
        wordGroups.Should().NotContain(group => excludedWords.Contains(group.WordInfo.Text));
    }

    [Test]
    public void WordGroupsWords_Should_BeUpperCase()
    {
        foreach (var group in wordGroups)
            AreLettersUpperCase(group.WordInfo.Text).Should().Be(true);
    }

    [Test]
    public void TagCloud_Should_CreateImageFile()
    {
        File.Exists(outputPath).Should().Be(true);
    }

    private static bool AreLettersUpperCase(string word)
    {
        return word.Where(char.IsLetter).All(letter => letter == char.ToUpper(letter));
    }
}