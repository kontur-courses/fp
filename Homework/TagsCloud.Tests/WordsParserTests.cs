using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsFilters;
using TagsCloud.Visualization.WordsParsers;

namespace TagsCloud.Tests
{
    public class WordsParserTests
    {
        private WordsParser sut;

        [SetUp]
        public void InitParser()
        {
            sut = new WordsParser(new IWordsFilter[] {new BoringWordsFilter()});
        }
        
        private static TestCaseData[] testCases =
        {
            new TestCaseData("", new List<Word>()).SetName("Empty text"),
            new TestCaseData("    ", new List<Word>()).SetName("Whitespace text"),
            new TestCaseData(", , , , ,", new List<Word>()).SetName("Only commas"),
            new TestCaseData("test test test", new List<Word> {new("test", 3)}).SetName("Simple text"),
            new TestCaseData("test Test TEST", new List<Word> {new("test", 3)})
                .SetName("Different case"),
            new TestCaseData("test,test,test", new List<Word> {new("test", 3)})
                .SetName("Separated by comma"),
            new TestCaseData("test\ntest\ntest", new List<Word> {new("test", 3)})
                .SetName("Separated by new line"),
            new TestCaseData("hello world hello world",
                    new List<Word> {new("hello", 2), new("world", 2)})
                .SetName("Two different words"),
            new TestCaseData("тест test",
                    new List<Word> {new("test", 1), new("тест", 1)})
                .SetName("Different languages"),
            new TestCaseData("1234 1234", new List<Word> {new("1234", 2)}).SetName("Digits"),
            new TestCaseData("Another brick in the wall",
                    new List<Word>
                    {
                        new("another", 1), new("brick", 1), new("wall", 1)
                    })
                .SetName("Boring words")
        };

        [TestCaseSource(nameof(testCases))]
        public void CountWordsFrequency_ShouldParseCorrectly_When(string text, List<Word> expectedResult)
        {
            sut.CountWordsFrequency(text)
                .Then(x => x.Should().Equal(expectedResult));
        }

        [Test]
        public void CountWordsFrequency_ShouldReturnFalseResult_OnNullInput()
        {
            sut.CountWordsFrequency(null).IsSuccess.Should().Be(false);
        }
    }
}