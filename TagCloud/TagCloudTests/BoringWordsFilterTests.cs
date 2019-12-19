using FluentAssertions;
using NUnit.Framework;
using System;
using TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    public class BoringWordsFilterTests
    {
        private BoringWordsFilter boringWordsFilter;
        private readonly BoringWord[] boringWords = 
        {
            new BoringWord("a"),
            new BoringWord("on"),
            new BoringWord("the"),
            new BoringWord("in")
        };
        private string[] allWords =
        {
            "josh",
            "on",
            "a",
            "bike",
            "crushed",
            "in",
            "the",
            "park"
        };

        [SetUp]
        public void BaseSetUp()
        {
            boringWordsFilter = new BoringWordsFilter(boringWords);
        }

        [Test]
        public void BoringWordsFilterShould_ThrowException_OnNullWords()
        {
            Action action = () => boringWordsFilter.FilterWords(null).GetValueOrThrow();
            action.Should().Throw<InvalidOperationException>().WithMessage("Error occured: Words cannot be null");
        }

        [Test]
        public void BoringWordsFilterShould_ReturnAllWords_OnEmptyBoringWords()
        {
            boringWordsFilter = new BoringWordsFilter(new BoringWord[] { });
            boringWordsFilter.FilterWords(allWords).GetValueOrThrow().Length
                .Should().Be(allWords.Length);
        }

        [Test]
        public void BoringWordsFilterShould_ReturnNoWords_OnEmptyWords()
        {
            boringWordsFilter.FilterWords(new string[] { }).GetValueOrThrow().Length.Should().Be(0);
        }

        [Test]
        public void BoringWordsFilterShould_FilterdWords()
        {
            boringWordsFilter.FilterWords(allWords).GetValueOrThrow().Length.Should().Be(4);
        }
    }
}
