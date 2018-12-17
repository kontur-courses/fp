using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.WordsProcessing;

namespace TagsCloudVisualization_Tests.WordProcessing
{
    public class WordsChanger_Should
    {
        private WordsChanger changer;

        [SetUp]
        public void SetUp()
        {
            changer = new WordsChanger();
        }

        [Test]
        public void ChangeWord_WhenLowerCase()
        {
            changer.ChangeWords(new []{"a"}).GetValueOrThrow().Should().BeEquivalentTo("a");
        }
    
        [Test]
        public void ChangeWord_WhenUpperCase()
        {
            changer.ChangeWords(new []{"A"}).GetValueOrThrow().Should().BeEquivalentTo("a");
        }

        [Test]
        public void ChangeWord_WhenUpperAndLowerCase()
        {
            changer.ChangeWords(new []{"Aa"}).GetValueOrThrow().Should().BeEquivalentTo("aa");
        }

        [Test]
        public void ChangeWord_WhenSeveralUpperCase()
        {
            changer.ChangeWords(new []{"AA"}).GetValueOrThrow().Should().BeEquivalentTo("aa");
        }
    }
}
