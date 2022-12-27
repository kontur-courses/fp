using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using TagCloud.TextParsing;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class TextParserTests
    {
        private TextParser parser;

        private List<string> exprectedWords = new List<string>() { "One", "two", "three", "four", "five", "six" };

        [SetUp]
        public void Setup()
        {
            parser = new TextParser();
        }

        [TestCase("One\ntwo\nthree\nfour\nfive\nsix\n", TestName = "Text with new line characters")]
        [TestCase("One\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix\r\n", TestName = "Text with new line and carriage return characters")]
        [TestCase("One two three four five six", TestName = "Text without punctuation marks")]
        [TestCase("One, two: three. four; five! - six?", TestName = "Text with punctuation marks")]
        public void GetWords_ReturnsCorrectWords(string text)
        {
            var actualWords = parser.GetWords(text);

            actualWords.IsSuccess.Should().BeTrue();
            actualWords.GetValueOrThrow().Should().BeEquivalentTo(exprectedWords);
        }

        [TestCase(null, TestName = "Text is null")]
        [TestCase("", TestName = "Text is empty")]
        public void GetWords_IsNotSuccess_WhenTextIsInvalid(string text)
        {
            var actualWords = parser.GetWords(text);

            actualWords.IsSuccess.Should().BeFalse();
        }
    }
}
