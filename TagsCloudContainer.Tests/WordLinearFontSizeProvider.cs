using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.WordColorProviders.Factories;
using TagsCloudContainer.Infrastructure.WordFontSizeProviders.Factories;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class WordLinearFontSizeProviderTests
    {
        private static IWordFontSizeProviderFactory fontSizeProviderFactory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fontSizeProviderFactory = new WordLinearFontSizeProviderFactory();
        }

        [Test]
        public void GetFontSize_Should_ReturnFailedResult_WhenWordDoesntExist()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var settings = new WordFontSizeSettings() { MinFrequencyFontSize = 14F, MaxFrequencyFontSize = 30F, WordFrequencies = wordFrequencies };
            var fontSizeProvider = fontSizeProviderFactory.CreateDefault(settings);

            var result = fontSizeProvider.GetFontSize("awesome");

            result.IsFailed.Should().BeTrue();
        }

        [Test]
        public void GetFontSize_Should_ReturnMinFrequencyFontSize_WhenWordHasMinFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var expectedFontSize = 14F;
            var settings = new WordFontSizeSettings() { MinFrequencyFontSize = expectedFontSize, MaxFrequencyFontSize = 30F, WordFrequencies = wordFrequencies };
            var fontSizeProvider = fontSizeProviderFactory.CreateDefault(settings);

            var actual = fontSizeProvider.GetFontSize("some");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(expectedFontSize);
        }

        [Test]
        public void GetFontSize_Should_ReturnMaxFrequencyFontSize_WhenWordHasMaxFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "text", 2 } };
            var expectedFontSize = 30F;
            var settings = new WordFontSizeSettings() { MinFrequencyFontSize = 14F, MaxFrequencyFontSize = expectedFontSize, WordFrequencies = wordFrequencies };
            var fontSizeProvider = fontSizeProviderFactory.CreateDefault(settings);

            var actual = fontSizeProvider.GetFontSize("text");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(expectedFontSize);
        }

        [Test]
        public void GetFontSize_Should_ReturnAverageFrequencyFontSize_WhenWordHasAverageFrequency()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "awesome", 2 }, { "text", 3 } };
            var expectedFontSize = 15F;
            var settings = new WordFontSizeSettings() { MinFrequencyFontSize = 10F, MaxFrequencyFontSize = 20F, WordFrequencies = wordFrequencies };
            var fontSizeProvider = fontSizeProviderFactory.CreateDefault(settings);

            var actual = fontSizeProvider.GetFontSize("awesome");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(expectedFontSize);
        }

        [Test]
        public void GetFontSize_Should_ReturnFrequencyFontSize_WhenMinFrequencyFontSizeEqualsToMax()
        {
            var wordFrequencies = new Dictionary<string, int>() { { "some", 1 }, { "awesome", 2 }, { "text", 3 } };
            var expectedFontSize = 15F;
            var settings = new WordFontSizeSettings() { MinFrequencyFontSize = expectedFontSize, MaxFrequencyFontSize = expectedFontSize, WordFrequencies = wordFrequencies };
            var fontSizeProvider = fontSizeProviderFactory.CreateDefault(settings);

            var actual = fontSizeProvider.GetFontSize("awesome");

            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().Be(expectedFontSize);
        }
    }
}