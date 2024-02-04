using FluentAssertions;
using TagsCloudPainter.FormPointer;
using TagsCloudPainter.Parser;
using TagsCloudPainter.Settings;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.FormPointer;

namespace TagsCloudPainterTests;

[TestFixture]
public class BoringTextParserTests
{
    [SetUp]
    public void Setup()
    {
        boringText = $"что{Environment.NewLine}и{Environment.NewLine}в";
        textSettings = new TextSettings { BoringText = boringText };
        boringTextParser = new BoringTextParser(textSettings);
    }

    private ITextSettings textSettings;
    private BoringTextParser boringTextParser;
    private string boringText;

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenGivenNullITextSettings()
    {
        Assert.Throws<ArgumentNullException>(() => new BoringTextParser(null));
    }

    [Test]
    public void ParseText_ShouldReturnWordsListWithoutBoringWords()
    {
        var boringWords = boringTextParser
            .GetBoringWords(
                boringText).GetValueOrThrow()
            .ToHashSet();
        var parsedText = boringTextParser.ParseText("Скучные Слова что в и").GetValueOrThrow();

        var isBoringWordsInParsedText = parsedText.Where(boringWords.Contains).Any();

        isBoringWordsInParsedText.Should().BeFalse();
    }

    [Test]
    public void ParseText_ShouldReturnNotEmptyWordsList_WhenPassedNotEmptyText()
    {
        var parsedText = boringTextParser.ParseText("Скучные Слова что в и").GetValueOrThrow();

        parsedText.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public void ParseText_ShouldReturnWordsInLowerCase()
    {
        var parsedText = boringTextParser.ParseText("Скучные Слова что в и").GetValueOrThrow();

        var isAnyWordNotLowered = parsedText.Any(word => !word.Equals(word, StringComparison.CurrentCultureIgnoreCase));

        isAnyWordNotLowered.Should().BeFalse();
    }

    [Test]
    public void ParseText_ShouldReturnWordsListWithTheSameAmountAsInText()
    {
        var parsedText = boringTextParser.ParseText("Скучные Слова что в и").GetValueOrThrow();

        parsedText.Count.Should().Be(2);
    }

    [Test]
    public void ParseText_ShouldFail_WhenGivenNullText()
    {
        var parsedText = boringTextParser.ParseText(null);

        Assert.That(parsedText.IsSuccess, Is.False);
    }

    [Test]
    public void ParseText_ShouldFail_WhenBoringTextIsNull()
    {
        textSettings.BoringText = null;

        var parsedText = boringTextParser.ParseText("Скучные Слова что в и");

        Assert.That(parsedText.IsSuccess, Is.False);
    }
}