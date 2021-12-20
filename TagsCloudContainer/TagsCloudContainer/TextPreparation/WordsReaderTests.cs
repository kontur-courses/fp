using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.TextPreparation
{
    public class WordsReaderTests
    {
        [Test]
        public void ReadAllWords_Fails_WhenFileContainsManyWordsInOneLine()
        {
            new WordsReader().ReadAllWords("a b c").Error.Should().Be("Each line must contain only one word");
        }

        [Test]
        public void ReadAllWords_ReturnsEmptyList_WhenFileIsEmpty()
        {
            new WordsReader().ReadAllWords("").GetValueOrThrow().Should().BeEmpty();
        }

        [Test]
        public void ReadAllWords_ReturnsEmptyList_WhenAllLinesAreEmpty()
        {
            new WordsReader().ReadAllWords(Environment.NewLine + Environment.NewLine + Environment.NewLine)
                .GetValueOrThrow()
                .Should()
                .BeEmpty();
        }

        [Test]
        public void ReadAllWords_AddsEachWordToResult()
        {
            var expectedResult = new List<string>() {"a", "b", "c"};

            new WordsReader().ReadAllWords("a" + Environment.NewLine + "b" + Environment.NewLine + "c")
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
        }

        [Test]
        public void ReadAllWords_NotAddsWordsSeparatedByLineBreak()
        {
            var expectedResult = new List<string>() {"a\nb\n"};

            new WordsReader().ReadAllWords("a\nb\n")
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
        }
    }
}