using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class MyStem_Should
    {
        private MyStem myStem;

        [SetUp]
        public void SetUp()
        {
            myStem = new MyStem();
        }

        [Test]
        public void NormalizeWords()
        {
            var words = new[] {"яблоко", "яблока", "яблоки"};
            var normWords = myStem.NormalizeWords(words).GetValueOrThrow().ToArray();
            normWords.Should().BeEquivalentTo("яблоко", "яблоко", "яблоко");
        }
    }
}