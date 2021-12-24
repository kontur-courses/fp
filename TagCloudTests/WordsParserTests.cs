using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.TextHandlers.Parser;

namespace TagCloudTests;

[TestFixture]
public class WordsParserTests
{
    private static readonly string[] Words = { "abc", "abcd", "1234", "test" };
    private const string Filename = "TestWords";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        File.WriteAllText($"{Filename}.txt", string.Join(Environment.NewLine, Words));
    }

    [Test]
    public void GetWords_ShouldReturnAllWordsFromFile()
    {
        var sut = new WordsByLineParser();

        var actualWords = sut.GetWords($"{Filename}.txt");

        actualWords.Should().BeEquivalentTo(Words.AsResult());
    }

    [TestCase("doc")]
    [TestCase("docx")]
    [TestCase("txt")]
    public void GetWords_CanReadOtherFormats(string fileExtension)
    {
        var filenameWithExtension = $"{Filename}.{fileExtension}";
        File.WriteAllText(filenameWithExtension, string.Join(Environment.NewLine, Words));
        var sut = new WordsByLineParser();

        var actualWords = sut.GetWords(filenameWithExtension);

        actualWords.Should().BeEquivalentTo(Words.AsResult());
    }
}