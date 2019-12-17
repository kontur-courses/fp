using System.Linq;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Logic;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TextParserTests
    {
        private TextParser textParser;

        [OneTimeSetUp]
        public void InitializeTextParser()
        {
            var defaultContainer = EntryPoint.InitializeContainer();
            textParser = defaultContainer.Resolve<IParser>() as TextParser;
        }

        [Test]
        public void TextParser_ReturnsFailResult_WhenTextIsNull()
        {
            textParser.ParseToTokens(null).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void TextParser_ReturnsFailResult_WhenTextIsEmpty()
        {
            var text = "";
            textParser.ParseToTokens(text).IsSuccess.Should().BeFalse();
        }

        [Test]
        public void TextParser_ReturnsCorrectResult_WithRegularTags()
        {
            var wordTags = new[] {"dog", "cat", "rat", "monkey"};
            var text = string.Join(" ", wordTags);
            textParser
                .ParseToTokens(text)
                .GetValueOrThrow()
                .Select(token => token.Word)
                .ToArray()
                .Should()
                .BeEquivalentTo(wordTags);
        }

        [Test]
        public void TextParser_ReturnsCorrectResult_WhenWordRepeatsManyTimes()
        {
            var wordTags = new[] {"dog", "dog", "dog", "dog"};
            var text = string.Join(" ", wordTags);
            var tokens = textParser.ParseToTokens(text).GetValueOrThrow().ToArray();

            tokens.Length.Should().Be(1);
            tokens.First().Should().BeEquivalentTo(new WordToken("dog", 4));
        }

        [TestCase("donut", "donut", TestName = "Common word")]
        [TestCase("lilian's", "lilian's", TestName = "Word with apostrophe")]
        [TestCase("foo-bar", "foo-bar", TestName = "Word with hyphen")]
        [TestCase("sh*t", "sh*t", TestName = "Word with asterisk")]
        [TestCase("snake_line", "snake_line", TestName = "2 words with underscore")]
        [TestCase("?!foo!&", "foo", TestName = "Separators around the word do not count to word")]
        public void TextParser_ReturnsSingleWordToken_FromGivenText(string inputText, string expectedWord)
        {
            var tokens = textParser.ParseToTokens(inputText).GetValueOrThrow().ToArray();

            tokens.Length.Should().Be(1);
            tokens.First().Word.Should().Be(expectedWord);
        }

        [Test]
        public void TextParser_SplitsTextToTokens_WithDifferentSeparatorCharacters()
        {
            var separators = new[] {'!', '@', '#', '?', '.', ',', '=', '^', '&', '%', '$'};
            var text = "foo" + string.Join("foo", separators) + "foo";

            textParser
                .ParseToTokens(text)
                .GetValueOrThrow()
                .First()
                .TextCount
                .Should()
                .Be(separators.Length + 1);
        }
    }
}