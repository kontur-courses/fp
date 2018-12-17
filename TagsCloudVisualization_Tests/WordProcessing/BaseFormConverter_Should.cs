using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.WordsProcessing;
// ReSharper disable StringLiteralTypo

namespace TagsCloudVisualization_Tests.WordProcessing
{
    public class BaseFormConverter_Should
    {
        private BaseFormConverter converter;

        [SetUp]
        public void SetUp()
        {
            var path = TestContext.CurrentContext.TestDirectory;
            converter = new BaseFormConverter($"{path}/Dictionaries/ru.aff", $"{path}/Dictionaries/ru.dic");
        }

        [Test]
        public void ChangeWordsToBase_WhenRussiaWord()
        {
            converter.ChangeWords(new[] {"сознания"}).GetValueOrThrow().Should().BeEquivalentTo("сознание");
        }

        [Test]
        public void ChangeWordsToBase_Not_WhenNotRussianWord()
        {
            converter.ChangeWords(new[] {"unchanged"}).GetValueOrThrow().Should().BeEquivalentTo("unchanged");
        }

        [Test]
        public void ChangeWordsToBase_OnlyRussia_WhenDifferentWord()
        {
            converter.ChangeWords(new[] {"сознания", "unchanged"}).GetValueOrThrow().Should().BeEquivalentTo("сознание", "unchanged");
        }

        [Test] public void ChangeWordsToBase_WhenMultipleRussiaWord()
        {
            converter.ChangeWords(new[] {"сознания", "горения"}).GetValueOrThrow().Should().BeEquivalentTo("сознание", "горение");
        }    
        [Test] public void ChangeWordsToBase_WhenProblemsOpeningFile()
        {
            converter = new BaseFormConverter("", "");
            converter.ChangeWords(new[] {"сознания", "горения"}).Error.Should().StartWith("Failed opening Hunspell Dictionaries");
        }    
    }
}
