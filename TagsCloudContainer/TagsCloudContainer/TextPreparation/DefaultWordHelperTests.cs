using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.TextPreparation
{
    public class DefaultWordHelperTests
    {
        private DefaultWordHelper sut;

        [SetUp]
        public void InitWordHelper()
        {
            sut = new DefaultWordHelper();
        }

        [Test]
        public void GetAllWordsToVisualize_Fails_WhenWordsIsNull()
        {
            sut.GetAllWordsToVisualize(null)
                .Error.Should().Be("Words can't be null");
        }

        [Test]
        public void GetAllWordsToVisualize_RemovesAllShortWords()
        {
            sut.GetAllWordsToVisualize(new List<string>() {"a", "bb", "ccc"}).GetValueOrThrow().Should().BeEmpty();
        }

        [Test]
        public void GetAllWordsToVisualize_ConvertAllWordsToLowerCase()
        {
            var expectedResult = new List<string>() {"aaaa", "bbbb"};

            sut.GetAllWordsToVisualize(new List<string>() {"AAAA", "bBBb"}).GetValueOrThrow().Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetAllWordsToVisualize_NotAddsRecurringWords()
        {
            var expectedResult = new List<string>() {"aaaa"};

            sut.GetAllWordsToVisualize(new List<string>() {"aaaa", "aaaa"}).GetValueOrThrow().Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetAllWordsToVisualize_NotAddsRecurringWordsInDifferentCases()
        {
            var expectedResult = new List<string>() {"aaaa"};

            sut.GetAllWordsToVisualize(new List<string>() {"aaaa", "aAaa"}).GetValueOrThrow().Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetAllWordsToVisualize_SortWordsByPopularity()
        {
            var expectedResult = new List<string>() {"aaaa", "bbbb", "cccc"};

            sut.GetAllWordsToVisualize(new List<string>()
                {
                    "aaaa",
                    "aaaa",
                    "bbbb",
                    "cccc",
                    "bbbb",
                    "aaaa"
                })
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetAllWordsToVisualize_SortWordsByOrderOfAppearance_WhenPopularityIsTheSame()
        {
            var expectedResult = new List<string>() {"aaaa", "bbbb"};

            sut.GetAllWordsToVisualize(new List<string>() {"aaaa", "bbbb", "aaaa", "bbbb"})
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo(expectedResult);
        }
    }
}