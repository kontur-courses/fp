using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TagsTests
    {
        [TestCase(null, 0.5f)]
        [TestCase("", 0.5f)]
        [TestCase("word", -1f)]
        [TestCase("word", 0f)]
        [TestCase("word", 1.1f)]
        public void Tag_ShouldNotBeCreated_WhenIncorrectInput(string word, float weight)
        {
            Tag.Create(word, weight).Error.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Tag_ShouldBeCreated_WhenCorrectInput()
        {
            Assert.DoesNotThrow(() => Tag.Create("word", 0.5f).GetValueOrThrow());
        }
    }
}