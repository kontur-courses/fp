using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Utils;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class WordSplitterTests
    {
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(",")]
        public void Split_ShouldReturnEmptyCollection_WhenTextHasNoAnyWord(string input)
        {
            WordSplitter.Split(input).Should().BeEmpty();
        }

        [TestCase("word", new[] { "word" })]
        [TestCase("first second", new[] { "first", "second" })]
        [TestCase("I'm not alone", new[] { "I'm", "not", "alone" })]
        [TestCase("Where are you, boy?", new[] { "Where", "are", "you", "boy" })]
        public void Split_ShouldReturnCorrectResult(string input, params object[] expectedWords)
        {
            var words = WordSplitter.Split(input);

            words.Should().ContainInOrder(expectedWords);
        }
    }
}