using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.TextHandlers.Parser;

namespace TagCloudTests
{
    [TestFixture]
    public class WordsParserTests
    {
        private static string[] words = { "abc", "abcd", "1234", "test" };
        private static string filename = "TestWords.doc";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            File.WriteAllText(filename, string.Join(Environment.NewLine, words));
        }

        [Test]
        public void GetWords_ShouldReturnAllWordsFromFile()
        {
            var sut = new WordsByLineParser();

            var actualWords = sut.GetWords(filename);

            actualWords.Should().BeEquivalentTo(words.AsResult());
        }

        [TestCase("doc")]
        [TestCase("docx")]
        [TestCase("txt")]
        public void GetWords_CanReadOtherFormats(string fileExtension)
        {
            File.WriteAllText($"test.{fileExtension}", string.Join(Environment.NewLine, words));
            var sut = new WordsByLineParser();

            var actualWords = sut.GetWords(filename);

            actualWords.Should().BeEquivalentTo(words.AsResult());
        }
    }
}