using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloudPainter.Common;
using TagCloudPainter.Lemmaizers;
using TagCloudPainter.Preprocessors;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Tests;

public class WordPreprocessorTest
{
    private WordPreprocessor WordPreprocessor { get; set; }
    private ParseSettings ParseSettings { get; set; }

    [SetUp]
    public void Setup()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Lemmaizers", "mystem.exe");

        var lemmaizer = new Lemmaizer(path);
        ParseSettings = new ParseSettings();

        WordPreprocessor =
            new WordPreprocessor(new ParseSettingsProvider { ParseSettings = ParseSettings }, lemmaizer);
    }

    [Test]
    public void GetWordsCountDictionary_Should_Fail_On_null()
    {
        var result = WordPreprocessor.GetWordsCountDictionary(null);
        result.Should().BeEquivalentTo(Result.Fail<Dictionary<string, int>>("words is null or Empty"));
    }

    [Test]
    public void GetWordsCountDictionary_Should_Fail_On_EmptyIEnumerable()
    {
        var result = WordPreprocessor.GetWordsCountDictionary(new List<string>());
        result.Should().BeEquivalentTo(Result.Fail<Dictionary<string, int>>("words is null or Empty"));
    }

    [TestCase("но", TestName = "{m}_CONJ")]
    [TestCase("ни", TestName = "{m}_PART")]
    [TestCase("через", TestName = "{m}_PR")]
    [TestCase("кто-то", TestName = "{m}_SPRO")]
    [TestCase("такой", TestName = "{m}_APRO")]
    [TestCase("там", TestName = "{m}_ADVPRO")]
    [TestCase("ой-ой-ой", TestName = "{m}_INTJ")]
    public void GetWordsCountDictionary_Should_Skip(string word)
    {
        var words = new List<string> { word };
        var result = WordPreprocessor.GetWordsCountDictionary(words).GetValueOrThrow();
        result.Should().BeEmpty();
    }

    [Test]
    public void GetWordsCountDictionary_Should_Skip_Given_Words()
    {
        ParseSettings.IgnoredWords.Add("слово1");
        ParseSettings.IgnoredWords.Add("слово2");
        ParseSettings.IgnoredWords.Add("слово3");

        var result = WordPreprocessor.GetWordsCountDictionary(ParseSettings.IgnoredWords.ToList()).GetValueOrThrow();

        result.Should().BeEmpty();
    }

    [Test]
    public void GetWordsCountDictionary_Should_Lowercase_Words()
    {
        var list = new List<string> { "Слово", "Идти" };

        var result = WordPreprocessor.GetWordsCountDictionary(list).GetValueOrThrow();

        result.Keys.ToList().Should().BeEquivalentTo(new List<string> { "слово", "идти" });
    }

    [Test]
    public void GetWordsCountDictionary_Should_Counting_Words()
    {
        var list = new List<string> { "Слово", "Слово", "Слово" };

        var result = WordPreprocessor.GetWordsCountDictionary(list).GetValueOrThrow();

        result.Count.Should().Be(1);
    }
}