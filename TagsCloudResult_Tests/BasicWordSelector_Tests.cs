using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult.Infrastructure.Common;
using TagsCloudResult;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class BasicWordSelectorTest
    {
        private static AppSettings defaultSetting = AppSettingsForTests.Settings;
        private static IEnumerable<string> words = new[] {"word", "another", "test", "or", "test", "word", "word"};

        [Test]
        public void ShouldReturnWords_IfGetCorrectInput()
        {
            var actual = BasicWordsSelector.Select(words, defaultSetting);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnUniqWords_IfGetCorrectInput()
        {
            var actual = BasicWordsSelector.Select(words, defaultSetting);

            actual.GetValueOrThrow().Should().OnlyHaveUniqueItems();
        }

        [Test]
        public void ShouldThrowAwayBoringWords_IfGetCorrectInput()
        {
            var actual = BasicWordsSelector.Select(words, defaultSetting);

            actual.GetValueOrThrow().Count().Should().Be(3);
        }

        [Test]
        public void ShouldScaleWords()
        {
            var actual = BasicWordsSelector.Select(words, defaultSetting);
            var wordsInOrder = actual.GetValueOrThrow().OrderBy(x => x.Size.Height).Select(x => x.Word).ToArray();

            wordsInOrder.Should().BeEquivalentTo(new[] {"another", "test", "word"});
        }

        [Test]
        public void ShouldWorkWithRandom()
        {
            var settings = new AppSettings(
                default(ImageSetting),
                new WordSetting("Arial", "random"),
                default(AlgorithmsSettings),
                "");
            var actual = BasicWordsSelector.Select(words, settings);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnError_WhenGetUnregisteredFont()
        {
            var settings = new AppSettings(
                default(ImageSetting),
                new WordSetting("asdaf", "Black"),
                default(AlgorithmsSettings),
                "");
            var actual = BasicWordsSelector.Select(words, settings);

            actual.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ShouldReturnError_WhenGetUnregisteredColor()
        {
            var settings = new AppSettings(
                default(ImageSetting),
                new WordSetting("Arial", "asd"),
                default(AlgorithmsSettings),
                "");
            var actual = BasicWordsSelector.Select(words, settings);

            actual.IsSuccess.Should().BeFalse();
        }
    }
}