using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsFilter;
using TagsCloud.Visualization.WordsParser;

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

        [TestCaseSource(typeof(TestDataGenerator))]
        public void CountWordsFrequency_Should_ParseCorrectly_When(string text, List<Word> expectedResult)
        {
            sut.CountWordsFrequency(text)
                .Then(x => x.Should().Equal(expectedResult));
        }

        [Test]
        public void CountWordsFrequency_Should_ReturnFalseResult_OnNullInput()
        {
            sut.CountWordsFrequency(null).IsSuccess.Should().Be(false);
        }
    }

    public class TestDataGenerator : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            yield return new TestCaseData("", new List<Word>()).SetName("Empty text");
            yield return new TestCaseData("    ", new List<Word>()).SetName("Whitespace text");
            yield return new TestCaseData(", , , , ,", new List<Word>()).SetName("Only commas");
            yield return new TestCaseData("test test test", new List<Word> {new("test", 3)}).SetName(
                "Simple text");
            yield return new TestCaseData("test Test TEST", new List<Word> {new("test", 3)}).SetName(
                "Different case");
            yield return new TestCaseData("test,test,test", new List<Word> {new("test", 3)}).SetName(
                "Separated by comma");
            yield return new TestCaseData("test\ntest\ntest", new List<Word> {new("test", 3)}).SetName(
                "Separated by new line");
            yield return new TestCaseData("hello world hello world",
                    new List<Word> {new("hello", 2), new("world", 2)})
                .SetName("Two different words");
            yield return new TestCaseData("тест test", new List<Word> {new("test", 1), new("тест", 1)})
                .SetName("Different languages");
            yield return new TestCaseData("1234 1234", new List<Word> {new("1234", 2)}).SetName("Digits");
            yield return new TestCaseData("Another brick in the wall",
                    new List<Word> {new("another", 1), new("brick", 1), new("wall", 1)})
                .SetName("Boring words");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}