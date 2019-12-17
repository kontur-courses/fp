using FluentAssertions;
using NUnit.Framework;
using System;
using TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    public class ParsersTests
    {
        private LowerCaseParser lowerCaseParser;
        private UpperCaseParser upperCaseParser;

        [SetUp]
        public void BaseSetUp()
        {
            lowerCaseParser = new LowerCaseParser();
            upperCaseParser = new UpperCaseParser();
        }

        [Test]
        public void LowerCaseParserShould_ThrowException_OnNullText()
        {
            Action action = () => lowerCaseParser.ParseWords(null).GetValueOrThrow();
            action.Should().Throw<Exception>().WithMessage("No value. Only Error: Words cannot be null");
        }

        [TestCase(new object[] { "" }, ExpectedResult = new string[] { "" })]
        [TestCase(new object[] { "a" }, ExpectedResult = new string[] { "a" })]
        [TestCase(new object[] { "A" }, ExpectedResult = new string[] { "a" })]
        [TestCase(new object[] { "AaA", "AAA" }, ExpectedResult = new string[] { "aaa", "aaa" })]
        public string[] LowerCaseParserShould_ReturnCorrectParsedWords(params string[] words)
        {
            return lowerCaseParser.ParseWords(words).GetValueOrThrow();
        }

        [Test]
        public void UpperCaseParserShould_ThrowException_OnNullText()
        {
            Action action = () => upperCaseParser.ParseWords(null).GetValueOrThrow();
            action.Should().Throw<Exception>().WithMessage("No value. Only Error: Words cannot be null");
        }

        [TestCase(new object[] { "" }, ExpectedResult = new string[] { "" })]
        [TestCase(new object[] { "a" }, ExpectedResult = new string[] { "A" })]
        [TestCase(new object[] { "A" }, ExpectedResult = new string[] { "A" })]
        [TestCase(new object[] { "AaA", "AAA" }, ExpectedResult = new string[] { "AAA", "AAA" })]
        public string[] UpperCaseParserShould_ReturnCorrectParsedWords(params string[] words)
        {
            return upperCaseParser.ParseWords(words).GetValueOrThrow();
        }
    }
}
